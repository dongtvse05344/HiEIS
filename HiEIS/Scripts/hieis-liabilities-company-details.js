$(document).ready(function (e) {
    //var year;
    ajaxGetYearByCompanyId();

    setTimeout(function () {
        console.log($('#year').val());
        $('.ibox-title h5').html("Chi tiết công nợ năm " + $('#year').val());
        table();
        moveComponents();
    }, 500);

    $('#year').on('change', function () {
        $('.ibox-title h5').html("Chi tiết công nợ năm " + $('#year').val());
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
});

function ajaxGetYearByCompanyId(data, callback, settings) {
    $.ajax({
        url: '/Public/Liabilities/GetYearByCompanyId',
        method: 'Post',
        data: {
            companyId: $('#liabilities-detail-table').data('id'),
        },
        success: function (res) {
            var year = res.listYear;
            if (year !== null) {
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
            url: '/Public/Liabilities/GetLiabilitiesDetailByCompanyId',
            method: 'Post',
            data: {
                searchPhase: data.search.value,
                page: data.start,
                pageSize: data.length,
                companyId: $('#liabilities-detail-table').data('id'),
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
                "data": "Note",
            }
        ]

    });
}

function getPayment() {
    var companyId = $('#liabilities-detail-table').data('id');
    var amount = $('#Amount').val().replace(/,/g, "");
    var note = $('#Note').val();
    var type = 2;

    var payment = {
        Type: type,
        Amount: amount,
        CompanyId: CompanyId,
        Note: note
    };

    return payment;

}