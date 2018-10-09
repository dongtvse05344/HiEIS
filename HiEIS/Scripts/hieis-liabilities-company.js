$(document).ready(function (e) {
    (function () {
        function ajaxLiabilitiesLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Public/Liabilities/GetCompanyLiabilities',
                method: 'Post',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    customerName: $('#search-name').val(),
                    customerAddress: $('#search-address').val(),
                    customerTel: $('#search-tel').val(),
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

        window.table = $('#liabilities-table').DataTable({
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
            "ajax": ajaxLiabilitiesLoading,
            "columns": [
                { "data": "Name" },
                {
                    "data": "Address",
                    "width": "30%"
                },
                {
                    "data": "Tel",
                    "className": "text-center"
                },
                {
                    "data": "Current",
                    "className": "text-center",
                    "orderable": false,
                    "render": function (data, type, row) {
                        var text = "";
                        if (row["Current"] == null) {
                            text = 0;
                        }
                        else {
                            text = numberWithCommas(data);
                        }
                        return text;
                    }
                },
                {
                    "data": "Payment",
                    "className": "text-center",
                    "orderable": false,
                    "render": function (data, type, row) {
                        var text = "";
                        if (row["Payment"] == null) {
                            text = 0;
                        }
                        else {
                            text = numberWithCommas(data);
                        }
                        return text;
                    }
                },
                {
                    "data": "Remain",
                    "className": "text-center",
                    "orderable": false,
                    "render": function (data, type, row) {
                        return numberWithCommas(data);
                    }
                },
                {
                    "data": null,
                    "className": 'table-action text-center',
                    "orderable": false,
                    render: function (data, type, row) {
                        var id = row.Id;
                        var btnView = '<a class="link" href="/Public/Liabilities/CustomerDetails/' + id + '"><i class="fa fa-eye"></i></a>';
                        return btnView;
                    }
                }
            ]

        });

        $('#search-name').on('keyup', function () {
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
});