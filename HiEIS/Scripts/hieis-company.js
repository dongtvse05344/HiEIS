$(document).ready(function (e) {
    toastr.options = {
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 1200
    };

    (function () {

        function ajaxcompanyLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Admin/Company/List',
                method: 'Post',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    compName: $('#search-name').val(),
                    compTaxNo: $('#search-tax-no').val(),
                    compAddress: $('#search-address').val(),
                    compTel: $('#search-tel').val(),
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

        window.table = $('#company-table').DataTable({
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
                    title: 'Doanh nghiệp',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                },
                {
                    extend: 'pdf',
                    title: 'Doanh nghiệp',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                }
            ],
            "processing": false,
            "serverSide": true,
            "ajax": ajaxcompanyLoading,
            "columns": [
                {
                    "data": "Name",
                    "width": "10%",
                    render: function (data, type, row) {
                        return "<b>" + data + "</b><br>MST: " + row.TaxNo
                    }
                },
                {
                    "data": "Address",
                    "width": "20%"
                },
                { "data": "Tel" },
                { "data": "Email" },
                { "data": "Website" },
                { "data": "Fax" },
                {
                    "data": null,
                    "className": 'table-action text-center',
                    "orderable": false,
                    render: function (data, type, row) {
                        var href = '';
                        var actionTemplate = $('#template-table-action').html();
                        actionTemplate = $(actionTemplate);

                        actionTemplate.find('.js-activate, .js-deactivate, .js-delete').attr('data-id', row.Id);

                        if (actionTemplate.find('.js-edit').length > 0) {
                            href = actionTemplate.find('.js-edit').attr('href');
                            href += '/' + row.Id;
                            actionTemplate.find('.js-edit').attr('href', href)
                        }

                        if (row["IsActive"] === true) {
                            actionTemplate.find('.js-activate').remove();
                        }
                        else {
                            actionTemplate.find('.js-deactivate').remove();
                        }
                        return actionTemplate.html();
                        //var id = row.Id;
                        //var roles = row.Roles;
                        //var btnEdit = '<a class="link" href="/Admin/Company/Edit/' + id + '"><i class="fa fa-pencil"></i></a>';
                        //var btnDeactivate = '<a class="link js-deactivate" data-id="' + id + '"><i class="fa fa-toggle-on"></i></a>';
                        //var btnActivate = '<a class="link js-activate" data-id="' + id + '"><i class="fa fa-toggle-off"></i></a>';
                        //var btnDelete = '<a data-id="' + id + '" class="link js-delete"><i class="fa fa-trash"></i></a>';
                        //return (row["IsActive"] === true ? btnDeactivate : btnActivate) + btnEdit + btnDelete;
                    }
                }
            ]

        });


        $('#search-name').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-tax-no').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-address').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-tel').on('keyup', function () {
            window.table.ajax.reload();
        });

    })();

    moveComponents();

    $('#company-table').on('click', '.js-delete', function (e) {

        e.preventDefault();
        var companyId = $(this).attr("data-id");

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
                    url: "/Admin/Company/Delete/",
                    data: { id: companyId },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#company-table').DataTable().ajax.reload();
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

    $('#company-table').on('click', '.js-activate', function (e) {

        e.preventDefault();
        var companyId = $(this).attr("data-id");

        $.ajax({
            url: "/Admin/Company/Activate/",
            data: { id: companyId },
            method: "POST",
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message, res.title);
                    $('#company-table').DataTable().ajax.reload();
                }
                else {
                    toastr.error(res.message, res.title);
                }
            },
            error: function (response) {
                toastr.error("Có lỗi xảy ra, vui lòng thử lại sau", "Không thành công");
            }
        });

    });

    $('#company-table').on('click', '.js-deactivate', function (e) {

        e.preventDefault();
        var companyId = $(this).attr("data-id");

        $.ajax({
            url: "/Admin/Company/Deactivate/",
            data: { id: companyId },
            method: "POST",
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message, res.title);
                    $('#company-table').DataTable().ajax.reload();
                }
                else {
                    swal(res.title, res.message, "error");
                }
            },
            error: function (response) {
                toastr.error("Có lỗi xảy ra, vui lòng thử lại sau", "Không thành công");
            }
        });

    });


    var jsMessage = sessionStorage['message'];
    if (jsMessage !== undefined) {
        toastr.success(jsMessage, "Thành công");
        sessionStorage.clear();
    }
});
