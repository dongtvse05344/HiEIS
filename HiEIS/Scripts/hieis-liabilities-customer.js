$(document).ready(function (e) {
    (function () {
        function ajaxLiabilitiesLoading(data, callback, settings) {
            $.ajax({
                url: '/Public/Liabilities/GetCustomerLiabilities',
                method: 'Post',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    companyName: $('#search-name').val(),
                    companyAddress: $('#search-address').val(),
                    companyTel: $('#search-tel').val()
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
                    "render": function (data, type, row) {
                        return numberWithCommas(data);
                    }
                },
                {
                    "data": null,
                    "width": "5%",
                    "className": 'table-action text-center',
                    "orderable": false,
                    render: function (data, type, row) {
                        var id = row.Id;
                        var btnView = '<a class="link" href="/Public/Liabilities/CompanyDetails/' + id + '"><i class="fa fa-eye"></i></a>';
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