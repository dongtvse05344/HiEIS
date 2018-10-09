$(document).ready(function (e) {
    //Customer Table
    (function () {
        //Customer Table
        function ajaxCustomerLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Public/Customer/List',
                method: 'Post',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    customerName: $('#search-customername').val(),
                    enterprise: $('#search-enterprise').val(),
                    taxNo: $('#search-tax').val(),
                    orderCol: orderCol,
                    orderDir: orderInfo.dir
                },
                success: function (res) {
                    //window.table.ajax.reload();
                    callback({
                        draw: data.draw,
                        recordsTotal: res.total,
                        recordsFiltered: res.display,
                        data: res.data
                    });
                }
            });
        }
        window.table = $('#customer-table').DataTable({
            "pageLength": '10',
            "responsive": true,
            "autoWidth": false,
            "processing": true,
            "serverSide": true,
            "ajax": ajaxCustomerLoading,
            "dom": 'Bfrtip',
            "buttons": [
                {
                    extend: 'copy'
                },
                {
                    extend: 'excel', title: 'Khách hàng'
                },
                {
                    extend: 'pdf', title: 'Khách hàng'
                }
            ],
            "columns": [
                { "data": "Name" },
                {
                    "data": "Enterprise",
                    "width": '25%'
                },
                { "data": "TaxNo" },
                {
                    "data": "Address",
                    "width": '20%'
                },
                { "data": "Tel" },
                {
                    "data": "Fax",
                    "width": '10%'
                },
                { "data": "Bank" },
                { "data": "BankAccountNumber" },
                {
                    "data": null,
                    "width": '10%',
                    "className": 'table-action text-center',
                    "render": function (data, type, row) {
                        var actionTemplate = $('#template-table-action').html();
                        actionTemplate = $(actionTemplate);

                        actionTemplate.find('.js-detail').attr({
                            'data-id': row.Id,
                            'data-name': row.Name
                        });
                        actionTemplate.find('.js-delete-customer').attr('data-id-customer', row.Id);

                        return actionTemplate.html();
                    }
                }

            ]

        });

        //Search Field
        $('#search-customername').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-enterprise').on('keyup', function () {
            window.table.ajax.reload();
        });

        $('#search-tax').on('keyup', function () {
            window.table.ajax.reload();
        });


    })();

    moveComponents();

    //Product Table (popup)
    APP_PUBLIC.ManageTemplates.init();


    //Popup Customer-Product detail + Addproduct
    $('#customer-table').on('click', '.js-detail', function (e) {
        var id = $(this).data('id');
        var name = $(this).data('name');
        window.customerId = id;
        window.customerName = name;

        window.productTable.ajax.reload();
        $("#modal-form-customerDetail").modal();
    });

    //Nhập đơn giá
    $('#product-price').keyup(function (event) {

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

    //Lưu sp
    $("#save").on("click", function (e) {
        e.preventDefault();
        $.ajax({
            url: '/Public/Customer/EditCustomerProduct',
            method: 'POST',
            data: {
                CustomerId: window.customer,
                ProductId: $("#product-list option:selected").val(),
                Amount: $("#product-amount").val()
            },
            success: function (res) {
                if (!res.success) {
                    validateProduct();
                } else {
                    window.productTable.ajax.reload();
                    clearProductInput();
                    swal('Thành công', res.message, 'success');
                }
            },
            fail: function () {
                swal('Thất bại', res.message, 'warning');
            }
        });
    });

    //Xóa sản phẩm
    $("#product-table").on("click", ".js-delete", function (e) {
        e.preventDefault();
        var _button = $(this);
        var productId = _button.data("id-product");
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
                    url: "/Public/Customer/DeleteCustomerProduct/",
                    data: {
                        CustomerId: window.customer,
                        ProductId: productId
                    },
                    method: "POST",
                    success: function (response) {
                        swal("Đã xóa", "Xóa sản phẩm thành công!", "success");
                        window.productTable.ajax.reload();
                    },
                    error: function (response) {
                        swal('Thất bại', response.message, 'warning');
                    }
                });
            }
            else {
                //Do nothing
            }

        });

    });

    //Chọn sản phẩm
    $("#product-list").on('change', function () {
        $selected = $("#product-list option:selected");
        var unit = $selected.data("unit");
        $("#product-amount").val(1);
        $("#product-unit").val(unit);
        $("#product-id").val($selected.val());

    });

    //Xóa khách hàng
    $("#customer-table").on("click", ".js-delete-customer", function (e) {
        e.preventDefault();
        var _button = $(this);
        var customerId = _button.data("id-customer");
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
                    url: "/Public/Customer/DeleteCustomer/",
                    data: {
                        id: customerId
                    },
                    method: "POST",
                    success: function (response) {
                        swal("Đã xóa", "Xóa khách hàng thành công!", "success");
                        window.table.ajax.reload();
                    },
                    error: function (response) {
                        swal('Thất bại', response.message, 'warning');
                    }
                });
            }
            else {
                //Do nothing
            }

        });

    });
});

//Create Product Datatable
APP_PUBLIC.ManageTemplates = function () {

    var initDetailDataTable = function () {

        function ajaxProductlLoading(data, callback, settings) {
            var orderInfo = data.order[0];
            var orderCol = data.columns[orderInfo.column].data;
            $.ajax({
                url: '/Public/Products/GetProductByCustomerId/' + window.customerId,
                method: 'GET',
                data: {
                    searchPhase: data.search.value,
                    page: data.start,
                    pageSize: data.length,
                    orderCol: orderCol,
                    orderDir: orderInfo.dir
                },
                success: function (res) {
                    $('#modal-form-customerDetail .modal-title').html(window.customerName);
                    window.customer = window.customerId;
                    callback({
                        draw: data.draw,
                        recordsTotal: res.total,
                        recordsFiltered: res.display,
                        data: res.data
                    });
                }
            });
        }

        window.productTable = $('#product-table').DataTable({
            //"pageLength": '10',
            "responsive": true,
            "autoWidth": false,
            "processing": true,
            "serverSide": true,
            "ajax": ajaxProductlLoading,
            "columns": [
                {
                    "data": "ProductName",
                    "width": "30%"
                },
                {
                    "data": "ProductUnit",
                    "className": "text-center"
                },
                //{
                //    "data": "ProductUnitPrice",
                //    "className": "text-center"
                //},
                {
                    "data": "ProductAmount",
                    "className": "text-center"
                },

                {
                    "data": null,
                    "className": "table-action text-center",
                    "orderable": false,
                    "render": function (data, type, row) {
                        var id = row.ProductId;
                        var btnDelete = '<a data-id-product="' + id + '" class="link js-delete"><i class="fa fa-trash"></i></a>';
                        return btnDelete;
                    }
                }
            ]
        });
    }

    return {
        init: initDetailDataTable
    }

}();

function clearProductInput() {
    $("#product-list").prop('selectedIndex', 0);
    $("#product-list").attr('disabled', false);
    $("#product-unit, #product-amount, #product-price, #product-vat").val("");
    $(".product-error, .price-error, .amount-error").html("");
}

function validateProduct() {
    var validated = true;

    if ($("#product-list").prop("selectedIndex") === 0) {
        $(".product-error").html("Vui lòng chọn sản phẩm.");
        validated = false;
    }


    var amount = $("#product-amount").val();
    if (amount === "" || amount <= 0 || amount === "e" || amount.indexOf('.') >= 0) {
        $(".amount-error").html("Vui lòng nhập số lượng > 1.");
        validated = false;
    }

    return validated;
}

