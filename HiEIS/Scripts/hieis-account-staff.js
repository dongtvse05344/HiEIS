$(document).ready(function () {
    toastr.options = {
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 1200
    };

    (function () {
        const STAFF_ROLE = {
            'Manager': 'Quản lý',
            'Accounting Manager': 'Kế toán trưởng',
            'Liability Accountant': 'Kế toán công nợ',
            'Payable Accountant': 'Kế toán thanh toán',
        }

        function ajaxAccountLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Admin/Account/GetAllStaffAccounts',
                method: 'POST',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    name: $('#search-name').val(),
                    userName: $('#search-username').val(),
                    email: $('#search-email').val(),
                    companyName: $('#search-company').val(),
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

        window.table = $('#staff-account-table').DataTable({
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
                    title: 'Tài khoản doanh nghiệp',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                },
                {
                    extend: 'pdf',
                    title: 'Tài khoản doanh nghiệp',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                }
            ],
            "processing": false,
            "serverSide": true,
            "ajax": ajaxAccountLoading,
            "columns": [
                {
                    "data": "Name",
                    "width": "5%"
                },
                {
                    "data": "UserName",
                    "className": "text-center",
                    "width": "7%"
                },
                {
                    "data": "CompanyName",
                    "width": "12%"
                },
                {
                    "data": "Address",
                    "width": "15%"
                },
                {
                    "data": "Email",
                    "width": "10%"
                },
                {
                    "data": null,
                    "width": "10%",
                    "orderable": false,
                    "render": function (data, type, row) {
                        var roles = row.Roles;
                        var text = '<ul class="role-list">';
                        roles.forEach(function (el) {
                            text += '<li>' + STAFF_ROLE[el.toString()] + '</li>';
                        });
                        return text + '</ul>';
                    }
                },
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
                        if (actionTemplate.find('.js-change-password').length > 0) {
                            href = actionTemplate.find('.js-change-password').attr('href');
                            href += '?id=' + row.Id;
                            href += '&url=' + window.location.pathname;
                            actionTemplate.find('.js-change-password').attr('href', href)
                        }
                        
                        if (row["AspNetUserIsActive"] === true) {
                            actionTemplate.find('.js-activate').remove();
                        }
                        else {
                            actionTemplate.find('.js-deactivate').remove();
                        }
                        return actionTemplate.html();
                    }
                }
            ]

        });

    })();

    moveComponents();

    $('#search-username').on('keyup', function () {
        window.table.ajax.reload();
    });

    $('#search-name').on('keyup', function () {
        window.table.ajax.reload();
    });

    $('#search-company').on('keyup', function () {
        window.table.ajax.reload();
    });

    $('#search-email').on('keyup', function () {
        window.table.ajax.reload();
    });

    $('#staff-account-table').on('click', '.js-activate', function (e) {

        e.preventDefault();
        var accId = $(this).data('id');

        $.ajax({
            url: "/Admin/Account/Activate/",
            data: { id: accId },
            method: "POST",
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message, res.title);
                    $('#staff-account-table').DataTable().ajax.reload();
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

    $('#staff-account-table').on('click', '.js-deactivate', function (e) {

        e.preventDefault();
        var accId = $(this).data('id');

        $.ajax({
            url: "/Admin/Account/Deactivate/",
            data: { id: accId },
            method: "POST",
            success: function (res) {
                if (res.success) {
                    toastr.success(res.message, res.title);
                    $('#staff-account-table').DataTable().ajax.reload();
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

    $('#staff-account-table').on('click', '.js-delete', function (e) {

        e.preventDefault();
        var accId = $(this).data('id');

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
                    url: "/Admin/Account/DeleteStaff/",
                    data: { id: accId },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#staff-account-table').DataTable().ajax.reload();
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
    
    var jsMessage = sessionStorage['message'];
    if (jsMessage !== undefined) {
        toastr.success(jsMessage, "Thành công");
        sessionStorage.clear();
    }

});