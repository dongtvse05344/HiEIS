APP_PUBLIC.ManageProducts = function () {
    // DATATABLE
    var initDataTable = function () {
        var tableId = '#product-table';
        var container = '.product';
        var downloadOptions = [
            {
                extend: 'copy',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'excel',
                title: 'Sản phẩm',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            },
            {
                extend: 'pdf',
                title: 'Sản phẩm',
                exportOptions: {
                    columns: ':not(:last-child)'
                }
            }
        ];

        // enable filter with column index
        var filterOptions = {
            price: 3
        }

        var ajaxUrl = '/Public/Products/GetProducts';
        var columnOptions = [
            { data: 'Name' },
            {
                data: 'Code',
                className: 'text-center'
            },
            {
                data: 'Unit',
                className: 'text-center'
            },
            {
                data: 'SUnitPrice',
                className: 'text-center'
            },
            {
                data: 'VATRate',
                className: 'text-center',
                render: function (data, type, row) {
                    var vatString = "";
                    
                    if (row.VATRate === -1) {
                        vatString = "Không chịu thuế";
                    }
                    else {
                        vatString = (row.VATRate * 100) + '%';
                    }
                    
                    return vatString;
                }
            },
            { data: null },
        ];

        var columnDefOptions = [
            {
                targets: -1, //last column
                className: 'table-action text-center',
                orderable: false,
                render: function (data, type, row) {
                    var actionTemplate = $('#template-table-action').html();
                    actionTemplate = $(actionTemplate);

                    actionTemplate.find('.js-delete').attr('data-id', row.Id);
                    if (actionTemplate.find('.js-edit').length > 0) {
                        href = actionTemplate.find('.js-edit').attr('href');
                        href += '/' + row.Id;
                        actionTemplate.find('.js-edit').attr('href', href)
                    }
                    
                    return actionTemplate.html();
                }
            }];

        APP_PUBLIC.DataTableCustom.initDataTable(tableId, container, downloadOptions, filterOptions, ajaxUrl, columnOptions, columnDefOptions);
    }
    // end DATATABLE

    return {
        init: initDataTable
    }
}();

$(document).ready(function (e) {
    //APP_PUBLIC.ManageProducts.init();

    (function () {

        function ajaxProductLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Public/Products/GetProducts',
                method: 'Post',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    productName: $('#search-name').val(),
                    productCode: $('#search-code').val(),
                    minPrice: $('#search-min').val().replace(/,/g, ""),
                    maxPrice: $('#search-max').val().replace(/,/g, ""),
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

        window.table = $('#product-table').DataTable({
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
                    title: 'Sản phẩm',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                },
                {
                    extend: 'pdf',
                    title: 'Sản phẩm',
                    exportOptions: {
                        columns: ':not(:last-child)'
                    }
                }
            ],
            "processing": false,
            "serverSide": true,
            "ajax": ajaxProductLoading,
            "columns": [
                { "data": "Name" },
                {
                    "data": "Code",
                    "className": "text-center"
                },
                {
                    "data": "Unit",
                    "className": "text-center"
                },
                {
                    "data": "SUnitPrice",
                    "className": "text-center"
                },
                {
                    "data": "VATRate",
                    "className": "text-center",
                    "render": function (data, type, row) {
                        var vatString = "";

                        if (row.VATRate === -1) {
                            vatString = "Không chịu thuế";
                        }
                        else {
                            vatString = (row.VATRate * 100) + '%';
                        }

                        return vatString;
                    }
                },
                {
                    "data": null,
                    "className": "table-action text-center",
                    "orderable": false,
                    "render": function (data, type, row) {
                        var actionTemplate = $('#template-table-action').html();
                        actionTemplate = $(actionTemplate);

                        actionTemplate.find('.js-delete').attr('data-id', row.Id);
                        if (actionTemplate.find('.js-edit').length > 0) {
                            href = actionTemplate.find('.js-edit').attr('href');
                            href += '/' + row.Id;
                            actionTemplate.find('.js-edit').attr('href', href)
                        }

                        return actionTemplate.html();
                    }
                }
            ]

        });


        $('#search-name, #search-code').on('keyup', function () {
            window.table.ajax.reload();
        });
        
        $('#search-min').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-max').on('keyup', function () {
            window.table.ajax.reload();
        });

    })();

    moveComponents();
    
    toastr.options = {
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 1200
    };

    $('#search-min, #search-max').keyup(function (event) {

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

    $("#product-table").on("click", ".js-delete", function (e) {

        e.preventDefault();
        var _button = $(this);
        var productId = _button.attr("data-id");

        swal({
            title: "Bạn có chắc không?",
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
                    url: "/Public/Products/Delete/",
                    data: { id: productId },
                    method: "POST",
                    success: function (response) {
                        swal("Đã xóa", "Xóa sản phẩm thành công!", "success");
                        $('#product-table').DataTable().ajax.reload();
                    },
                    error: function (response) {
                        // false
                    }
                });
            }
            else {
                //Do nothing
            }

        });

    });
});

// ========================

function create() {
    var product = $('#formCreateProduct').serialize();
    console.log(product);

    $.ajax({
        url: '/Public/Products/CreateProduct',
        method: 'POST',
        data: product,
        dataType: 'JSON',
        success: function (response) {
            // true
            console.log(response);
            $('#product-table').DataTable().ajax.reload();
        },
        error: function (response) {
            // false
        }
    });
}
