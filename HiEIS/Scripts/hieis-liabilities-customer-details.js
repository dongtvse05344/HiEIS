$(document).ready(function (e) {
    toastr.options = {
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 1200
    };

    //var year;
    ajaxGetYear();

    setTimeout(function () {
        console.log($('#year').val());
        $('.detail-title h5').html("Chi tiết công nợ năm " + $('#year').val());
        table();
        moveComponents();
    }, 500);

    $('#year').on('change', function () {
        $('.detail-title h5').html("Chi tiết công nợ năm " + $('#year').val());
        window.table.ajax.reload();
    });

    $('#Amount').keyup(function (event) {
        // skip for arrow keys
        if (event.which >= 37 && event.which <= 40) return;

        // format number
        $(this).val(function (index, value) {
            return value
                .replace(/\D/g, "")
                .replace(/\B(?=(\d{3})+(?!\d))/g, ",")
                ;
        });
    });

    $('#close-modal').on('click', function (){
        clearInput();
        $('.amount-error').html("");
    });

    $('#save').on('click', function () {
        $('.amount-error').html("");
        var amount = parseInt($('#Amount').val().replace(/,/g, ""));
        var remain = parseInt($('#Remain').val().replace(/,/g, ""));
        if (amount > remain) {
            $('.amount-error').html("Nợ chỉ còn " + numberWithCommas(remain) + ". Vui lòng kiểm tra lại số tiền!");
            $('#Amount').focus();
        } else {
            var payment = getPayment();
            console.log(payment);

            $.ajax({
                url: "/Public/Liabilities/CreateTransaction/",
                data: {
                    model: payment
                },
                method: "POST",
                success: function (res) {
                    if (!res.success) {
                        if (res.data && res.data.length) {
                            $('.field-validation-error').empty();
                            res.data.forEach(e => {
                                $('.field-validation-error').html(e.error);
                            });
                            swal('Không thành công', res.message, 'warning');
                        } else {
                            swal('Không thành công', res.message, 'error');
                        }
                    } else {
                        clearInput();
                        $('#modal-form').modal('hide');
                        window.table.ajax.reload();
                        toastr.success(res.message, "Thành công");
                    }
                },
                error: function (response) {
                    swal('Không thành công', 'Tạo thông báo phí không thành công. Vui lòng thử lại!', 'error');
                    console.log("Fail!!!!!!!!!!!!!!!!!!!!!!!");
                }
            });
        }

    });
    
});

function ajaxGetYear(data, callback, settings) {
    $.ajax({
        url: '/Public/Liabilities/GetYear',
        method: 'Post',
        data: {
            id: $('#liabilities-detail-table').data('id'),
        },
        success: function (res) {
            var year = res.listYear;
            if (year != null) {
                var options = [];
                year.forEach(e => {
                    var opt = $('<option>', {
                        'value': e,
                        'html': e,
                    }).data('opt', e);
                    options.push(opt);
                });
                $('#year').html(options);
            }
        }
    });
}

function table() {
    const TYPE = {
        '1': 'Nợ',
        '2': 'Trả',
    }
    const TYPE_CLASS = {
        '1': 'label-warning',
        '2': 'label-primary'
    }

    function ajaxLiabilitiesDetailsLoading(data, callback, settings) {
        var orderInfo = data.order[0];
        var orderCol = data.columns[orderInfo.column].data;
        $.ajax({
            url: '/Public/Liabilities/GetLiabilitiesDetail',
            method: 'Post',
            data: {
                searchPhase: data.search.value,
                page: data.start,
                pageSize: data.length,
                customerId: $('#liabilities-detail-table').data('id'),
                year: $('#year').val(),
                orderCol: orderCol,
                orderDir: orderInfo.dir
            },
            success: function (res) {
                var trans = res.transaction;
                $('#Current').val(numberWithCommas(res.transaction.Current));
                $('#LastRemain').val(numberWithCommas(res.transaction.LastRemain));
                $('#TotalCurrent').val(numberWithCommas(res.transaction.TotalCurrent));
                $('#Paid').val(numberWithCommas(res.transaction.Payment));
                $('#Remain').val(numberWithCommas(res.transaction.Remain));

                if ($('#year').val() == new Date().getFullYear()) {
                    if (res.transaction.Remain == 0) {
                        $(".payment-btn").hide();
                    } else {
                        $(".payment-btn").show();
                    }
                } else {
                    $(".payment-btn").hide();
                }


                callback({
                    draw: data.draw,
                    recordsTotal: res.data.total,
                    recordsFiltered: res.data.display,
                    data: res.data.data
                });
            }
        });
    }

    window.table = $('#liabilities-detail-table').DataTable({
        "pageLength": '10',
        "responsive": true,
        "autoWidth": false,
        "processing": false,
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
                title: 'Công Nợ',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'pdf',
                title: 'Công Nợ',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            }
        ],
        "serverSide": true,
        "ajax": ajaxLiabilitiesDetailsLoading,
        "columns": [
            {
                "data": "Date",
                "className": "text-center",
                "render": function (data, type, row) {
                    return moment(data).format('DD/MM/YYYY');
                }
            },

            {
                "data": "Type",
                "className": "text-center",
                render: function (data, type, row) {
                    return '<span class="label '
                        + TYPE_CLASS[data.toString()]
                        + '">'
                        + TYPE[data.toString()]
                        + '</span>';
                }
            },
            {
                "data": "Amount",
                "className": "text-center",
                "render": function (data, type, row) {
                    var text = "";
                    if (row["Amount"] == null) {
                        text = 0;
                    }
                    else {
                        text = numberWithCommas(data);
                    }
                    return text;
                }
            },
            {
                "data": "Note"
            }
        ]

    });
}

function getPayment() {
    var customerId = $('#liabilities-detail-table').data('id');
    var amount = $('#Amount').val().replace(/,/g, "");
    var note = $('#Note').val();
    var type = 2;

    var payment = {
        Type: type,
        Amount: amount,
        CustomerId: customerId,
        Note: note
    };

    return payment;

}

function clearInput() {
    $('#Amount').val("");
    $('#Note').val("");
}

function visiblePaymentBtn() {

    


}