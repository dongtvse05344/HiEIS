var invoiceObj = [];

const VAT_RATE = {
    '0.00': '0%',
    '0.05': '5%',
    '0.10': '10%',
    '-1.00': 'Không chịu thuế'
}

function clearInput() {
    $("#TaxNo, #Name, #Enterprise, #Address, #Bank, #BankAccountNumber, #Email, #Note, #Tel, #Fax, #CustomerId").val("");
    $("#Form, #Serial, #PaymentMethod").prop("selectedIndex", 0);
    $("#HasDueDate").iCheck('uncheck');
    $('#DueDate').datepicker('setDate', null);

    $("#TaxNo, #Name, #Enterprise, #Address, #Bank, #BankAccountNumber, #Email, #Tel, #Fax").attr("readonly", false);
    allowEnterpriseTypeahead();
}

function clearProductInput() {
    $("#product-list").prop('selectedIndex', 0);
    $("#product-list").attr('disabled', false);
    $("#product-unit, #product-quantity, #product-price, #product-vat").val("");
    $(".product-error, .price-error, .quantity-error").html("");
}

function validateProduct() {
    var validated = true;

    if ($("#product-list").prop("selectedIndex") === 0) {
        $(".product-error").html("Vui lòng chọn sản phẩm.");
        validated = false;
    }
    if ($("#product-price").val() === "") {
        $(".price-error").html("Vui lòng nhập đơn giá.");
        validated = false;
    }
    var quantity = $("#product-quantity").val();
    if (quantity === "" || quantity <= 0 || quantity === "e" || quantity.indexOf('.') >= 0) {
        $(".quantity-error").html("Vui lòng nhập số lượng > 1.");
        validated = false;
    }

    return validated;
}

function capitalize(string) {
    return string.charAt(0).toUpperCase() + string.slice(1).toLowerCase();
}

function calculateTotalAmount() {
    var subTotal = 0;

    $.each(invoiceObj, function (index, value) {
        var money = value.Total;
        subTotal += parseInt(money);
    });

    var vatRate = $("#VATRate").val();
    if (vatRate == -1) {
        vatRate = 0;
    }
    var vatAmount = subTotal * vatRate;
    var totalAmount = subTotal + vatAmount;
    var amountInWords = capitalize(DOCSO.doc(totalAmount) + ' đồng');

    $("#SubTotal").val(numberWithCommas(subTotal));
    $("#VATAmount").val(numberWithCommas(vatAmount));
    $("#Total").val(numberWithCommas(totalAmount));
    $("#AmountInWords").val(amountInWords);
}

function getInvoice() {
    var dueDate = null;
    var vatRate = parseFloat($('#VATRate').val());
    var customerId = $('#CustomerId').val();
    var template = $('#Template option:selected').text().split("-").map(item => item.trim());

    if ($('#HasDueDate').is(':checked')) {
        //dueDate = moment($('#DueDate').val(), 'DD/MM/YYYY').format('MM/DD/YYYY');
        dueDate = $('#DueDate').val();
    }

    var invoice = {
        DueDateString: dueDate,
        PaymentMethod: parseInt($('#PaymentMethod').val()),
        SubTotal: parseFloat($('#SubTotal').val().replace(/,/g, "")),
        VATRate: vatRate,
        VATAmount: parseFloat($('#VATAmount').val().replace(/,/g, "")),
        Note: $('#Note').val(),
        Total: parseFloat($('#Total').val().replace(/,/g, "")),
        AmountInWords: $('#AmountInWords').val(),
        TemplateId: parseInt($("#Template").val()),
        TemplateForm: template[0],
        TemplateSerial: template[1],
        Name: $("#Name").val(),
        Enterprise: $("#Enterprise").val(),
        Address: $("#Address").val(),
        TaxNo: $("#TaxNo").val(),
        Tel: $("#Tel").val(),
        Fax: $("#Fax").val(),
        Email: $("#Email").val(),
        BankAccountNumber: $("#BankAccountNumber").val(),
        Bank: $("#Bank").val(),
        CustomerId: $("#CustomerId").val(),
        InvoiceItems: invoiceObj
    };

    return invoice;
}

function renderItemTable() {
    $table = $("#invoice-item-table tbody");
    $table.empty();

    $.each(invoiceObj, function (index, value) {
        var template = $('#template-product-row').html();
        template = $(template);

        template.find('.no').text(index + 1);
        template.find('.name').text(value.ProductName);
        template.find('.unit').text(value.ProductUnit);
        template.find('.quantity').text(value.Quantity);
        template.find('.unit-price').text(numberWithCommas(value.UnitPrice));
        template.find('.total').text(numberWithCommas(value.Total));
        template.find('.js-item-update, .js-item-delete').attr('data-selected', value.ProductId);

        $table.append(template[0].outerHTML);
    });
}

function allowEnterpriseTypeahead() {
    //Autocomplete
    $('#Enterprise').typeahead(
        {
            highlight: true
        },
        {
            name: 'customer-enterprise',
            display: function (item) {
                return item.Enterprise;
            },
            source: function (query, sync, callback) {
                $.ajax({
                    url: '/Public/Customer/List',
                    method: 'POST',
                    dataType: "json",
                    data: {
                        searchPhase: query,
                        page: 0,
                        pageSize: 10
                    },
                    success: function (res) {
                        callback(res.data);
                    }
                });
                return false;
            }
        }).bind("typeahead:select", function (e, customer) {
            $('#TaxNo').val(customer.TaxNo);
            $('#Name').val(customer.Name);
            $('#Address').val(customer.Address);
            $('#CustomerId').val(customer.Id);
            if (customer.Tel !== null && customer.Tel !== "") {
                $('#Tel').val(customer.Tel);
                $('#Tel').attr("readonly", true);
            }
            if (customer.Fax !== null && customer.Fax !== "") {
                $('#Fax').val(customer.Fax);
                $('#Fax').attr("readonly", true);
            }
            if (customer.AspNetUserEmail !== null && customer.AspNetUserEmail !== "") {
                $('#Email').val(customer.AspNetUserEmail);
                $('#Email').attr("readonly", true);
            }
            if (customer.Bank !== null && customer.Bank !== "") {
                $('#Bank').val(customer.Bank);
                $('#Bank').attr("readonly", true);
            }
            if (customer.BankAccountNumber !== null && customer.BankAccountNumber !== "") {
                $('#BankAccountNumber').val(customer.BankAccountNumber);
                $('#BankAccountNumber').attr("readonly", true);
            }

            $('#Enterprise').typeahead('destroy');
            $("#TaxNo, #Name, #Address, #Enterprise").attr("readonly", true);
        });
}

var DOCSO = function () { var t = ["không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín"], r = function (r, n) { var o = "", a = Math.floor(r / 10), e = r % 10; return a > 1 ? (o = " " + t[a] + " mươi", 1 == e && (o += " mốt")) : 1 == a ? (o = " mười", 1 == e && (o += " một")) : n && e > 0 && (o = " lẻ"), 5 == e && a >= 1 ? o += " lăm" : 4 == e && a >= 1 ? o += " tư" : (e > 1 || 1 == e && 0 == a) && (o += " " + t[e]), o }, n = function (n, o) { var a = "", e = Math.floor(n / 100), n = n % 100; return o || e > 0 ? (a = " " + t[e] + " trăm", a += r(n, !0)) : a = r(n, !1), a }, o = function (t, r) { var o = "", a = Math.floor(t / 1e6), t = t % 1e6; a > 0 && (o = n(a, r) + " triệu", r = !0); var e = Math.floor(t / 1e3), t = t % 1e3; return e > 0 && (o += n(e, r) + " ngàn", r = !0), t > 0 && (o += n(t, r)), o }; return { doc: function (r) { if (0 == r) return t[0]; var n = "", a = ""; do ty = r % 1e9, r = Math.floor(r / 1e9), n = r > 0 ? o(ty, !0) + a + n : o(ty, !1) + a + n, a = " tỷ"; while (r > 0); return n.trim() } } }();

$(document).ready(function () {
    
    toastr.options = {
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 2300
    };

    $("#DueDate").datepicker({
        format: 'dd/mm/yyyy',
        keyboardNavigation: false,
        forceParse: false,
        autoclose: true,
        startDate: new Date()
    });

    $('#HasDueDate').on('ifUnchecked', function (event) {
        $("#DueDate").val(null);
    });

    //Autocomplete
    allowEnterpriseTypeahead();

    //Lấy MST
    $("#btn-tax-no").on("click", function () {
        var tax = $("#TaxNo").val();
        if (tax === "") {
            toastr.warning("Vui lòng nhập mã số thuế.", "Nhắc nhở");
        }
        else {
            $.ajax({
                url: '/Public/Invoices/GetEnterpriseInfoByTaxNo',
                data: { taxNo: tax },
                method: 'GET',
                contentType: "application/json; charset=utf-8",
                dataType: 'json',
                success: function (response) {
                    if (response.IsDelete === true || response.MaSoThue === null) {
                        toastr.error("Mã số thuế không tồn tại.", "Không thành công");
                        $("#Enterprise").val("");
                        $("#Address").val("");
                    }
                    else {
                        $("#TaxNo").val(response.MaSoThue);
                        $("#Enterprise").val(response.Title);
                        $("#Address").val(response.DiaChiCongTy);
                        $("#CustomerId").val("");

                        $('#Enterprise').typeahead('destroy');
                        $("#TaxNo, #Enterprise, #Address").attr("readonly", true);
                    }
                },
                error: function (response) {
                    toastr.error("Có lỗi xảy ra, vui lòng thử lại sau.", "Không thành công");
                }
            });
        }

    });

    //Mở pop thêm sản phẩm
    $("#choose-products").on("click", function () {
        $('#save').data('action-type', 'create');
        if ($("#Address").val() === "") {
            toastr.warning("Vui lòng nhập Địa chỉ.", "Nhắc nhở");
        }
        else {
            $("#modal-form").modal();
            clearProductInput();
            $("#save").data("create");
        }
    });

    //Chọn sản phẩm
    $("#product-list").on('change', function () {
        $selected = $("#product-list option:selected");
        var unitPrice = $selected.data("price");
        var unit = $selected.data("unit");
        var vat = $selected.data("vat");

        $("#product-quantity").val(1);
        $("#product-unit").val(unit);
        $("#product-price").val(unitPrice);
        $("#product-vat").val(VAT_RATE[vat.toString()]);
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
    $("#save").on("click", function () {
        var actionType = $(this).data("action-type");
        var rowIndex = $(this).data("row");

        $(".product-error").html("");
        $(".price-error").html("");
        $(".quantity-error").html("");

        if (validateProduct()) {
            var productId = parseInt($("#product-list").val());
            var productVAT = parseFloat($("#product-list option:selected").data("vat"));
            var quantity = parseInt($("#product-quantity").val());
            var price = parseFloat($("#product-price").val().replace(/,/g, ""));
            var total = quantity * price;

            if (actionType === "create") {
                var item = invoiceObj.filter(e => e.ProductId == productId);
                if (item.length) {
                    swal('Sản phẩm đã tồn tại', 'Bạn đã thêm sản phẩm này vào hóa đơn rồi.', 'warning');
                }
                else {
                    var obj = {
                        ProductId: productId,
                        ProductName: $("#product-list option:selected").text(),
                        ProductUnit: $("#product-unit").val(),
                        Quantity: quantity,
                        UnitPrice: price,
                        VATRate: productVAT,
                        Total: total
                    };
                    invoiceObj.push(obj);

                    renderItemTable();
                    clearProductInput();
                }

            }
            else if (actionType === 'update') {
                var item = invoiceObj.filter(e => e.ProductId == productId);
                if (item.length) {
                    item[0].Quantity = quantity;
                    item[0].UnitPrice = price;
                    item[0].Total = total;
                }

                renderItemTable();
                $('#modal-form').modal('toggle');
            }

            calculateTotalAmount();
        }
    });

    //Chọn sp update
    $('#invoice-item-table').on('click', '.js-item-update', function () {
        var productId = $(this).data('selected');
        var item = invoiceObj.filter(e => e.ProductId == productId);
        if (item.length) {
            var vat = parseFloat(item[0].VATRate).toFixed(2);
            $('#modal-form').modal();
            $('#product-list').val(productId);
            $('#product-list').attr('disabled', true);
            $('#product-unit').val(item[0].ProductUnit);
            $('#product-price').val(numberWithCommas(item[0].UnitPrice));
            $('#product-quantity').val(item[0].Quantity);
            $('#product-vat').val(VAT_RATE[vat]);
            $('#save').data('action-type', 'update');
        }
    });

    //Xóa sp
    $("#invoice-item-table").on("click", ".js-item-delete", function () {
        //e.preventDefault();
        var $button = $(this);

        swal({
            title: "Bạn có chắc không?",
            type: "warning",
            showCancelButton: true,
            confirmButtonColor: "#DD6B55",
            confirmButtonText: "Có",
            cancelButtonText: "Hủy",
            closeOnConfirm: false
        }, function (isConfirm) {
            if (isConfirm) {
                var productId = $button.data('selected');
                invoiceObj = $.grep(invoiceObj, function (e) {
                    return e.ProductId != productId;
                });

                renderItemTable();
                calculateTotalAmount();
                swal("Đã xóa", null, "success");
            }
            else {
                //Do nothing
            }

        });

    });

    //Chọn thuế
    $("#VATRate").on("change", function () {
        calculateTotalAmount();
    });
});

