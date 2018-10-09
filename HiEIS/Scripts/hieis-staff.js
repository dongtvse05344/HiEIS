$(document).ready(function (e) {

    (function () {

        const STAFF_ROLE = {
            'Manager': 'Quản lý',
            'Accounting Manager': 'Kế toán trưởng',
            'Liability Accountant': 'Kế toán công nợ',
            'Payable Accountant': 'Kế toán thanh toán',
        }

        function ajaxStaffLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Public/Staffs/List',
                method: 'Post',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    staffName: $('#search-name').val(),
                    staffCode: $('#search-code').val(),
                    staffUsername: $('#search-username').val(),
                    staffTel: $('#search-tel').val(),
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

        window.table = $('#staff-table').DataTable({
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
                    title: 'Nhân viên',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                },
                {
                    extend: 'pdf',
                    title: 'Nhân viên',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                }
            ],
            "processing": false,
            "serverSide": true,
            "ajax": ajaxStaffLoading,
            "columns": [
                { "data": "Name" },
                {
                    "data": "Code",
                    "className": "text-center"
                },
                {
                    "data": "UserName",
                    "className": "text-center"
                },
                {
                    "data": "Address",
                    "width": "15%"
                },
                { "data": "Email" },
                { "data": "Tel" },
                {
                    "data": null,
                    "width": "15%",
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
                    "render": function (data, type, row) {
                        var id = row.Id;
                        var roles = row.Roles;
                        var isManager = false;
                        roles.forEach(function (el) {
                            if (el === "Manager") {
                                isManager = true;
                            }
                        });

                        var actionTemplate = $('#template-table-action').html();
                        actionTemplate = $(actionTemplate);

                        actionTemplate.find('.js-delete').attr('data-id', row.Id);
                        if (actionTemplate.find('.js-edit').length > 0) {
                            href = actionTemplate.find('.js-edit').attr('href');
                            href += '/' + row.Id;
                            actionTemplate.find('.js-edit').attr('href', href)
                        }
                        if (isManager) {
                            actionTemplate.find('.js-delete').addClass('disabled');
                        }

                        return actionTemplate.html();
                    }
                }
            ]

        });


        $('#search-name').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-code').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-username').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-tel').on('keyup', function () {
            window.table.ajax.reload();
        });

    })();

    moveComponents();

    $('#staff-table').on('click', '.js-delete', function (e) {

        e.preventDefault();
        var _button = $(this);
        var staffId = _button.attr("data-id");

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
                    url: "/Public/Staffs/Delete/",
                    data: { id: staffId },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#staff-table').DataTable().ajax.reload();
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
