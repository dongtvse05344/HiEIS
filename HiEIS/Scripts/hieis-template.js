APP_PUBLIC.ManageTemplates = function () {

    const TEMPLATE_ISACTIVE = {
        'true': 'Đang sử dụng',
        'false': 'Không sử dụng'
    };
    const STATUS_CLASSES = {
        'false': 'label-default',
        'true': 'label-primary'
    };

    // DATATABLE
    var initDataTable = function () {
        var tableId = '#template-table';
        var container = '.template';
        var downloadOptions = [
            {
                extend: 'copy'
            },
            {
                extend: 'excel', title: 'Hóa đơn'
            },
            {
                extend: 'pdf', title: 'Hóa đơn'
            }
        ];

        // enable filter with column index
        var filterOptions = {
            date: 3
        };

        var ajaxUrl = '/Public/templates/GetAllTemplates';
        var columnOptions = [
            {
                data: 'Name'

            },
            {
                data: 'Form',
                className: 'text-center',
                orderable: false
            },
            {
                data: 'Serial',
                className: 'text-center',
                orderable: false
            },

            {
                data: 'IsActive',
                className: 'text-center',
                orderable: false,
                render: function (data, type, row) {
                    return '<span class="label '
                        + STATUS_CLASSES[data.toString()]
                        + '">'
                        + TEMPLATE_ISACTIVE[data.toString()]
                        + '</span>';
                }
            },
            { data: null }
        ];

        var columnDefOptions = [
            {
                targets: -1, //last column
                className: 'table-action text-center',
                orderable: false,
                render: function (data, type, row) {
                    var actionTemplate = $('#template-table-action').html();
                    actionTemplate = $(actionTemplate);

                    actionTemplate.find('.js-template').attr('data-file-url', row['FileUrl']);
                    actionTemplate.find('.js-detail-template').attr('data-template-id', row['Id']);

                    return actionTemplate.html();
                }
            },
            {
                targets: 0,
                orderable: false
            }
        ];
        console.log(1);
        APP_PUBLIC.DataTableCustom.initDataTable(tableId, container, downloadOptions, filterOptions, ajaxUrl, columnOptions, columnDefOptions);
    };
    // end DATATABLE


    //Template Detail
    var initDetailDataTable = function () {
        $('#modal-form-template form').on('submit', function (e) {
            var data = new FormData(e.target);
            var url = $(e.target).attr('action');
            e.preventDefault();
            $.ajax({
                url: url,
                data: data,
                processData: false,
                contentType: false,
                method: 'post',
                success: function (res) {
                    if (!res.success) {
                        if (res.data && res.data.length) {
                            $('#modal-form-template .field-validation-error').empty();
                            res.data.forEach(e => {
                                $('#modal-form-template [name=' + e.name + ']~.field-validation-error').html(e.error);
                            });
                            swal('Thất bại', res.message, 'warning');
                        } else {
                            swal('Thất bại', res.message, 'error');
                        }
                    } else {
                        window.templateTable.ajax.reload();
                        clearInput();
                        swal('Thành công', res.message, 'success');
                    }
                },
                fail: function () {

                }
            });
        });

        function ajaxTemplateDetailLoading(data, callback, settings) {
            $.ajax({
                url: '/Public/Templates/TemplateDetail/' + window.templateId,
                method: 'GET',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    customer: ''
                },
                success: function (res) {
                    $('#modal-form-template .field-validation-error').empty();
                    var firstRow = res.data[0];
                    $('#modal-form-template .modal-title').html(firstRow.Form + ' - ' + firstRow.Serial);
                    $('#modal-form-template #Form').val(firstRow.Form);
                    $('#modal-form-template #Serial').val(firstRow.Serial);
                    $('#modal-form-template #Name').val(firstRow.Name);
                    $('#modal-form-template #Id').val(firstRow.Id);
                    callback({
                        draw: data.draw,
                        recordsTotal: res.total,
                        recordsFiltered: res.display,
                        data: res.data
                    });
                }
            });
        }

        window.templateTable = $('#template-detail-table').DataTable({
            "pageLength": '10',
            "responsive": true,
            "autoWidth": false,
            "processing": true,
            "serverSide": true,
            "ajax": ajaxTemplateDetailLoading,
            "columns": [
                {
                    "data": "Amount",
                    "className": "text-center",
                    "orderable": false
                },
                {
                    "data": "BeginNo",
                    "className": "text-center",
                    "orderable": false
                },
                {
                    "data": "CurrentNo",
                    "className": "text-center",
                    "orderable": false
                },
                {
                    "data": "ReleaseDate",
                    "className": "text-center",
                    "orderable": false,
                    "render": function (data, ) {
                        return moment(data).format('DD/MM/YYYY');
                    }
                },
                {
                    "data": "IsActive",
                    "className": "text-center",
                    "orderable": false,
                    "render": function (data, ) {
                        return '<span class="label '
                            + STATUS_CLASSES[data.toString()]
                            + ' templateStatus">'
                            + TEMPLATE_ISACTIVE[data.toString()]
                            + '</span>';
                    }
                },
                {
                    "data": null,
                    "className": "table-action text-center",
                    "orderable": false,
                    "render": function (data, type, row) {
                        var id = row.Id;
                        var status = row['IsActive'];
                        var actionTemplate = $('#detail-table-action').html();
                        actionTemplate = $(actionTemplate);
                        actionTemplate.find('.js-announcement').attr('href', row['ReleaseAnnouncementUrl']);
                        var btnAnnouncement = actionTemplate.html();
                        var btnDelete;

                        if (!status) {
                            btnDelete = '<a class="link js-delete" template-id="' + id + '" data-toggle="tooltip" title="Xóa mẫu hóa đơn"><i class="fa fa-trash"></i></a>';

                        } else {
                            btnDelete = '<a class="link js-delete disabled" template-id="' + id + '" data-toggle="tooltip" title="Xóa mẫu hóa đơn"><i class="fa fa-trash"></i></a>';
                        }
                        return btnDelete + btnAnnouncement;
                    }
                }
            ]
        });
    };

    return {
        init: initDataTable,
        initDetail: initDetailDataTable
    };

}();


$(document).ready(function (e) {
    APP_PUBLIC.ManageTemplates.init();

    moveComponents();

    APP_PUBLIC.ManageTemplates.initDetail();

    //Popup Mẫu hóa đơn
    $("#template-table").on("click", ".js-template", function (e) {
        e.preventDefault();
        var fileUrl = $(this).attr("data-file-url");
        console.log(fileUrl);
        $("#modal-form iframe").attr("src", fileUrl);
    });

    //Popup Template detail
    $("#template-table").on("click", ".js-detail-template", function (e) {
        var id = $(this).attr("data-template-id");
        window.templateId = id;
        window.templateTable.ajax.reload();
        $("#modal-form-template").modal();

        setTimeout(function () {
            $('.my-tooltip.center .tooltip-text').each(function (index, value) {
                var halftWidth = $(this).width() / 2;
                var parentHalfWidth = $(this).parent().width() / 2;
                var padding = parseFloat($(this).css('padding-left'));
                $(this).css({
                    'transform': 'translateX(-' + (halftWidth - parentHalfWidth + padding) + 'px)'
                });
            });
        }, 1000);
    });


    $("#template-detail-table").on("click", ".js-delete", function (e) {
        e.preventDefault();
        var _button = $(this);
        var templateID = _button.attr("template-id");

        swal({
            title: "Bạn có chắc chắn xóa không?",
            text: "Bạn không thể hoàn tác lại hành động này!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Hủy",
            closeOnConfirm: false
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: "/Public/Templates/Delete/",
                    data: { id: templateID },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#template-detail-table').DataTable().ajax.reload();
                        }
                        else {
                            swal(res.title, res.message, "error");
                        }
                    },
                    error: function (response) {
                        swal("Không thành công", "Có lỗi xảy ra, vui lòng thử lại sau!", "error");
                    }
                });
            }
            else {
                //Do nothing
            }

        });
    });



});
var date = new Date();
date.setDate(date.getDate());


//Release date
$('#release-date').datepicker({
    format: "dd/mm/yyyy",
    startDate: date,
    keyboardNavigation: false,
    forceParse: false,
    autoclose: true,
    onSelect: function () {
        table.draw();
    }
}).attr('readonly', 'readonly').datepicker("setDate", "0");


//clear input
function clearInput() {

    $('#modal-form-template #Amount').val("");
    $('#modal-form-template #ReleaseAnnounmentFile').val("");
    $('#release-date').datepicker('setDate', "0");
}