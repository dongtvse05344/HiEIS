const PROFORMAINVOICE_STATUS = {
    '1': 'Mới tạo',
    '2': 'Đã duyệt',
}
const STATUS_CLASSES = {
    '1': 'label-info',
    '2': 'label-primary'
}

$(document).ready(function (e) {

    //APP_PUBLIC.ManageProformaInvoices.init();

    $('#min, #max').datepicker({
        format: 'dd/mm/yyyy',
        keyboardNavigation: false,
        forceParse: false,
        autoclose: true,
        onSelect: function () {
            table.draw();
        }
    });

    (function () {

        function ajaxProformaLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Public/ProformaInvoice/GetProformaInvoices',
                method: 'Post',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    customerName: $('#search-name').val(),
                    lookupCode: $('#search-code').val(),
                    minDate: $('#min').val(),
                    maxDate: $('#max').val(),
                    statusString: $('#search-status').val(),
                    orderCol: orderCol,
                    orderDir: orderInfo.dir
                },
                success: function (res) {
                    callback({
                        draw: data.draw,
                        recordsTotal: res.total,
                        recordsFiltered: res.display,
                        data: res.data
                    });
                }
            });
        }

        window.table = $('#proforma-table').DataTable({
            "pageLength": '10',
            "responsive": true,
            "autoWidth": false,
            "dom": 'Bfrtip',
            "buttons": [
                {
                    extend: 'copy',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                },
                {
                    extend: 'excel',
                    title: 'Thông báo phí',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                },
                {
                    extend: 'pdf',
                    title: 'Thông báo phí',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                }
            ],
            "processing": false,
            "serverSide": true,
            "ajax": ajaxProformaLoading,
            "columns": [
                {
                    data: 'CustomerName',
                    render: function (data, type, row) {
                        var text = '';
                        if (row['CustomerName'] !== null) {
                            text = '<b>' + row['CustomerName'] + '</b><br>'
                                + row['CustomerEnterprise'];
                        }
                        else {
                            text = (row['Name'] === null ? "" : ('<b>' + row['Name'] + '</b><br>'))
                                + (row['Enterprise'] === null ? row['Address'] : row['Enterprise']);
                        }
                        return text;
                    }
                },
                {
                    data: 'LookupCode',
                    render: function (data, type, row) {
                        var text = '<a class="js-pdf" data-file-url="'
                            + row['FileUrl']
                            + '" data-toggle="modal" href="#modal-form" data-modal-link="">'
                            + row['LookupCode']
                            + "</a>";
                        return text;
                    }
                },
                {
                    data: 'Date',
                    className: 'text-center',
                    render: function (data, type, row) {
                        return moment(data).format('DD/MM/YYYY');
                    }
                },
                {
                    data: 'PaymentDeadline',
                    className: 'text-center',
                    render: function (data, type, row) {
                        return moment(data).format('DD/MM/YYYY');
                    }
                },
                {
                    data: 'Status',
                    className: 'text-center',
                    render: function (data, type, row) {
                        return '<span class="label '
                            + STATUS_CLASSES[data.toString()]
                            + '">'
                            + PROFORMAINVOICE_STATUS[data.toString()]
                            + '</span>';
                    }
                },
                {
                    data: 'Total',
                    className: 'text-center',
                    render: function (data, type, row) {
                        return data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                    }
                },

                {
                    data: null,
                    className: 'table-action text-center',
                    width: '20%',
                    orderable: false,
                    render: function (data, type, row) {
                        var href = '';
                        var status = row['Status'];
                        var actionTemplate = $('#template-table-action').html();
                        actionTemplate = $(actionTemplate);
                        actionTemplate.find('.link').attr('data-id', row.Id);
                        if (actionTemplate.find('.js-edit').length > 0) {
                            href = actionTemplate.find('.js-edit').attr('href');
                            href += '/' + row.Id;
                            actionTemplate.find('.js-edit').attr('href', href)
                        }
                        if (actionTemplate.find('.js-convert').length > 0) {
                            href = actionTemplate.find('.js-convert').attr('href');
                            href += '/' + row.Id;
                            actionTemplate.find('.js-convert').attr('href', href)
                        }
                        if (status === 2) {
                            actionTemplate.find('.js-edit, .js-delete, .js-confirm').addClass("disabled");
                        }

                        if (status === 1) {

                            actionTemplate.find('.js-send, .js-convert').addClass("disabled");
                        }

                        return actionTemplate.html();
                    }
                }
            ],
            "order": [[2, "desc"]]
        });


        $('#search-name, #search-code').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#min, #max, #search-status').on('change', function () {
            window.table.ajax.reload();
        });

    })();

    moveComponents();

    $('[data-toggle="tooltip"]').tooltip();

    $("#proforma-table").on("click", ".js-delete", function (e) {

        e.preventDefault();
        var _button = $(this);
        var productId = _button.attr("data-id");

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
                    url: "/Public/ProformaInvoice/DeleteProformaInvoice/",
                    data: { id: productId },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#proforma-table').DataTable().ajax.reload();
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

    $("#proforma-table").on("click", ".js-confirm", function (e) {
        e.preventDefault();
        var _button = $(this);
        var proformaId = _button.attr("data-id");

        swal({
            title: "Bạn muốn duyệt thông báo phí?",
            text: "Thông báo phí sẽ được duyệt. Bạn không thể hoàn tác lại hành động này!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Hủy",
            closeOnConfirm: false
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: "/Public/ProformaInvoice/ConfirmProforma/",
                    data: { id: proformaId },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#proforma-table').DataTable().ajax.reload();
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

    $('#proforma-table').on('click', '.js-pdf', function (e) {
        e.preventDefault();
        var fileUrl = $(this).attr("data-file-url");
        $("#modal-form embed").attr("src", fileUrl);
    });

    toastr.options = {
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 1200
    };

    var jsMessage = sessionStorage['message'];
    if (jsMessage != undefined) {
        toastr.success(jsMessage, "Thành công");
        sessionStorage.clear();
    }

    $('#proforma-table').on('click', '.js-send', function (e) {
        e.preventDefault();
        var invoiceId = $(this).attr('data-id');

        $.ajax({
            url: "/Public/ProformaInvoice/GetProformaEmail",
            data: { id: invoiceId },
            method: "POST",
            success: function (res) {
                $('#customer-email').val(res.email);
            },
            error: function (res) {

            }
        });
        $("#modal-email").modal();
        $('#send-email').attr('data-id', invoiceId);
    });

    $('#send-email').on('click', function () {
        var email = $('#customer-email').val();
        if (email === "") {
            swal("Không thành công", "Vui lòng nhập địa chỉ email!", "warning");
        }
        else {
            var invoiceId = $(this).attr('data-id');

            $.ajax({
                url: "/Public/ProformaInvoice/SendEmail",
                data: {
                    id: invoiceId,
                    email: email
                },
                method: "POST",
                success: function (res) {
                    if (res.success) {
                        $('#modal-email').modal('hide');
                        swal(res.title, res.message, "success");
                    }
                    else {
                        swal(res.title, res.message, "error");
                    }
                },
                error: function (res) {
                    swal("Không thành công", "Có lỗi xảy ra, vui lòng thử lại sau!", "Error");
                }
            });
        }

    });
    
});

//
APP_PUBLIC.ManageProformaInvoices = function () {


    // PROFORMA INVOICE DATATABLE
    var initDataTable = function () {
        var tableId = '#proforma-table';
        var container = '.proforma';
        var downloadOptions = [
            {
                extend: 'copy',
                exportOptions: {
                    columns: 'not:(:last-child)'
                }
            },
            {
                extend: 'excel', title: 'Thông báo phí',
                exportOptions: {
                    columns: 'not:(:last-child)'
                }
            },
            {
                extend: 'pdf', title: 'Thông báo phí',
                exportOptions: {
                    columns: 'not:(:last-child)'
                }
            }
        ];

        // enable filter with column index
        var filterOptions = {
            date: 2,
            status: 4
        }

        var ajaxUrl = '/Public/ProformaInvoice/GetProformaInvoices';
        var columnOptions = [
            { data: null },
            { data: null },
            {
                data: 'Date',
                className: 'text-center',
                render: function (data, type, row) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                data: 'PaymentDeadline',
                className: 'text-center',
                render: function (data, type, row) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },
            {
                data: 'Status',
                className: 'text-center',
                render: function (data, type, row) {
                    return '<span class="label '
                        + STATUS_CLASSES[data.toString()]
                        + '">'
                        + PROFORMAINVOICE_STATUS[data.toString()]
                        + '</span>';
                }
            },
            {
                data: 'Total',
                className: 'text-center',
                render: function (data, type, row) {
                    return data.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
                }
            },

            { data: null }
        ];

        var columnDefOptions = [
            {
                targets: 0,
                render: function (data, type, row) {
                    var text = '';
                    if (row['CustomerName'] !== null) {
                        text = '<b>' + row['CustomerName'] + '</b><br>'
                            + row['CustomerEnterprise'];
                    }
                    else {
                        text = (row['Name'] === null ? "" : ('<b>' + row['Name'] + '</b><br>'))
                            + (row['Enterprise'] === null ? row['Address'] : row['Enterprise']);
                    }
                    return text;
                }
            },
            {
                targets: 1,
                render: function (data, type, row) {
                    var text = '<a class="js-pdf" data-file-url="'
                        + row['FileUrl']
                        + '" data-toggle="modal" href="#modal-form" data-modal-link="">'
                        + row['LookupCode']
                        + "</a>";
                    return text;
                }
            },
            {
                targets: -1, //last column
                className: 'table-action text-center',
                width: '20%',
                orderable: false,
                render: function (data, type, row) {
                    var href = '';
                    var status = row['Status'];
                    var actionTemplate = $('#template-table-action').html();
                    actionTemplate = $(actionTemplate);
                    actionTemplate.find('.link').attr('data-id', row.Id);
                    if (actionTemplate.find('.js-edit').length > 0) {
                        href = actionTemplate.find('.js-edit').attr('href');
                        href += '/' + row.Id;
                        actionTemplate.find('.js-edit').attr('href', href)
                    }
                    if (actionTemplate.find('.js-convert').length > 0) {
                        href = actionTemplate.find('.js-convert').attr('href');
                        href += '/' + row.Id;
                        actionTemplate.find('.js-convert').attr('href', href)
                    }
                    if (status === 2) {
                        actionTemplate.find('.js-edit, .js-delete, .js-confirm').addClass("disabled");
                    }

                    if (status === 1) {
                        
                        actionTemplate.find('.js-send, .js-convert').addClass("disabled");
                    }
                    
                    return actionTemplate.html();
                }
            }
        ];

        APP_PUBLIC.DataTableCustom.initDataTable(tableId, container, downloadOptions, filterOptions, ajaxUrl, columnOptions, columnDefOptions);
    }
    // end DATATABLE

    return {
        init: initDataTable
    }

}();