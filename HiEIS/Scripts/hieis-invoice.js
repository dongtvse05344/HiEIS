const INVOICE_STATUS = {
    '1': 'Mới tạo',
    '2': 'Chờ ký',
    '3': 'Đã ký',
}
const STATUS_CLASSES = {
    '1': 'label-info',
    '2': 'label-warning',
    '3': 'label-primary'
}
const PAYMENT_STATUS = {
    'true': 'Đã thanh toán',
    'false': 'Chưa thanh toán'
}

$(document).ready(function (e) {

    
    //APP_PUBLIC.ManageInvoices.init();

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

        function ajaxInvoiceLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Public/Invoices/GetInvoices',
                method: 'Post',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    customerName: $('#search-name').val(),
                    lookupCode: $('#search-code').val(),
                    minString: $('#min').val(),
                    maxString: $('#max').val(),
                    paymentString: $('#search-payment').val(),
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

        window.table = $('#invoice-table').DataTable({
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
                    title: 'Hóa đơn',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                },
                {
                    extend: 'pdf',
                    title: 'Hóa đơn',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                }
            ],
            "processing": false,
            "serverSide": true,
            "ajax": ajaxInvoiceLoading,
            "columns": [
                {
                    data: 'Name',
                    render: function (data, type, row) {
                        var text = '';
                        if (row['CustomerName'] != null) {
                            text = '<b>' + row['CustomerName'] + '</b><br>'
                                + row['CustomerEnterprise'];
                        }
                        else {
                            text = (row['Name'] == null ? "" : ('<b>' + row['Name'] + '</b><br>'))
                                + (row['Enterprise'] == null ? row['Address'] : row['Enterprise']);
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
                    data: 'TemplateForm',
                    className: 'text-center',
                    render: function (data, type, row) {
                        return data + '<br/>' + row['TemplateSerial'];
                    }
                },
                {
                    data: 'Number',
                    className: 'text-center'
                },
                {
                    data: 'PaymentStatus',
                    className: 'text-center',
                    render: function (data, type, row) {
                        return PAYMENT_STATUS[data.toString()];
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
                    data: 'DueDate',
                    className: 'text-center',
                    render: function (data, type, row) {
                        if (data == null || data == "") {
                            return "";
                        }
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
                            + INVOICE_STATUS[data.toString()]
                            + '</span>';
                    }
                },
                {
                    data: null,
                    className: 'table-action text-center',
                    orderable: false,
                    render: function (data, type, row) {
                        var status = row['Status'];
                        var actionTemplate = $('#template-table-action').html();
                        actionTemplate = $(actionTemplate);
                        actionTemplate.find('.link').attr('data-id', row.Id);
                        if (actionTemplate.find('.js-edit').length > 0) {
                            var href = actionTemplate.find('.js-edit').attr('href');
                            href += '/' + row.Id;
                            actionTemplate.find('.js-edit').attr('href', href)
                        }
                        if (status === 2 || status === 3) {
                            actionTemplate.find('.js-edit, .js-delete, .js-confirm-payment').addClass("disabled");
                        }
                        //if (status === 1 || status === 3) {
                        //    actionTemplate.find('.js-sign').addClass("disabled");
                        //}

                        return actionTemplate.html();
                    }
                }
            ],
            "order": [[5, "desc"]]
        });


        $('#search-name, #search-code').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#min, #max, #search-payment, #search-status').on('change', function () {
            window.table.ajax.reload();
        });

    })();

    moveComponents();

    $('[data-toggle="tooltip"]').tooltip();

    $('#invoice-table').on('click', '.js-pdf', function (e) {
        e.preventDefault();
        var fileUrl = $(this).attr("data-file-url");
        $("#modal-form embed").attr("src", fileUrl /*+ "?dummy=" + Math.random()*/);
    });

    $('#invoice-table').on('click', '.js-delete', function (e) {

        e.preventDefault();
        var _button = $(this);
        var invoiceId = _button.attr("data-id");

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
                    url: "/Public/Invoices/Delete/",
                    data: { id: invoiceId },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#invoice-table').DataTable().ajax.reload();
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

    $('#invoice-table').on('click', '.js-confirm-payment', function (e) {

        e.preventDefault();
        var _button = $(this);
        var invoiceId = _button.attr("data-id");

        swal({
            title: "Bạn muốn xác nhận thanh toán?",
            text: "Hóa đơn sẽ được đánh số. Bạn không thể hoàn tác lại hành động này!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Hủy",
            closeOnConfirm: false
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: "/Public/Invoices/ConfirmPayment/",
                    data: { id: invoiceId },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#invoice-table').DataTable().ajax.reload();
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

    $('#invoice-table').on('click', '.js-sign', function (e) {
        e.preventDefault();
        var invoiceId = $(this).attr('data-id');

        swal({
            title: "Bạn muốn phát hành hóa đơn?",
            text: "Hóa đơn sẽ được ký chữ ký số. Bạn không thể hoàn tác lại hành động này!",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Hủy",
            closeOnConfirm: false
        }, function (isConfirm) {
            if (isConfirm) {
                $.ajax({
                    url: "/Public/Invoices/Sign/",
                    data: { id: invoiceId },
                    method: "POST",
                    success: function (res) {
                        if (res.success) {
                            swal(res.title, res.message, "success");
                            $('#invoice-table').DataTable().ajax.reload();
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

    $('#invoice-table').on('click', '.js-send-email', function (e) {
        e.preventDefault();
        var invoiceId = $(this).attr('data-id');

        $.ajax({
            url: "/Public/Invoices/GetInvoiceEmail",
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
                url: "/Public/Invoices/SendEmail",
                data: {
                    id: invoiceId,
                    email: email
                },
                method: "POST",
                success: function (res) {
                    if (res.success) {
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