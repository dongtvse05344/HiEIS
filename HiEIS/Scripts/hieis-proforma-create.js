var proformaObj = [];
const VAT_RATE = {
    '0': '0%',
    '0.05': '5%',
    '0.1': '10%',
    '-1': 'Không chịu thuế'
}

const VAT = {
    '0%': '0',
    '5%': '0.05',
    '10%': '0.10',
    'Không chịu thuế': '-1.00'
}


function clearInput() {
    $("#Enterprise, #Address, #Tel, #CustomerId, #Liabilities").val("");
    $('#PaymentDeadline').datepicker('setDate', "0");

    $("#Enterprise, #Address, #Tel").attr("readonly", false);
    allowEnterpriseTypeahead();
}

function clearProductInput() {
    $("#product-list").prop('selectedIndex', 0);

    $("#product-list").attr('disabled', false);
    $("#product-oldNumber, #product-newNumber, #product-price").attr('disabled', true);
    $("#product-unit, #product-price, #product-vat, #product-oldNumber, #product-newNumber, #product-weight, #product-VATAmount, #product-sub-total, #product-total").val("");
    $(".product-error, .price-error, .newNumber-error, .oldNumber-error, .time-to-error, .time-from-error").html("");
    $('#product-time-from, #product-time-to').datepicker('setDate', null);
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
            $('#Address').val(customer.Address);
            $('#CustomerId').val(customer.Id);
            if (customer.Tel !== null && customer.Tel !== "") {
                $('#Tel').val(customer.Tel);
                $('#Tel').attr("readonly", true);
            }

            $('#Enterprise').typeahead('destroy');
            $("#Address, #Enterprise").attr("readonly", true);
            if (customer.Id != null && customer.Id !== "") {
                ajaxGetLiabilities();
            }


        });
}

function numberWithCommas(number) {
    var parts = number.toString().split(".");
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");
    return parts.join(".");
}

//Get Liabilities of Customer
function ajaxGetLiabilities(data, callback, settings) {
    $.ajax({
        url: '/Public/Customer/GetLiabilitiesByCustomerId',
        method: 'Post',
        data: {
            id: $('#CustomerId').val(),
        },
        success: function (res) {
            var money = res.liabilities;
            if (money == "") {
                money = 0;
            }
            $('#Liabilities').val(money);
        }
    });
}

//Get OldNumber of Product
function ajaxGetOldNumber(data, callback, settings) {
    $.ajax({
        url: '/Public/Products/GetOldNumber',
        method: 'Post',
        data: {
            productId: $("#product-list").val(),
            customerId: $('#CustomerId').val()
        },
        success: function (res) {
            if (res.number == "-1" || res.number == null) {
                $('#product-oldNumber').val("");
            } else {
                $('#product-oldNumber').val(res.number);
                $('#product-newNumber').attr({ "min": parseInt(res.number) + 1 });
            }
        }
    });
}

//Get Products of Customer
function ajaxGetProducts(data, callback, settings) {
    $.ajax({
        url: '/Public/Products/GetProductByCustomerId',
        method: 'Post',
        data: {
            id: $('#CustomerId').val(),
            pageSize: -1,
            page: 0,
            searchPhase: ''
        },
        success: function (res) {
            var options = [$('<option>', {
                'html': '----- Chọn sản phẩm -----'
            })];
            res.data.forEach(e => {
                var opt = $('<option>', {
                    'value': e.ProductId,
                    'html': e.ProductName,
                }).data('opt', e);
                options.push(opt);
            });
            $('#product-list').html(options);
        }

    });
}

//Tính sản phẩm
function productCalculate() {

    var newNumber = parseInt($('#product-newNumber').val());
    var oldNumber = parseInt($('#product-oldNumber').val());
    var unitPrice = parseFloat($('#product-price').val().replace(/,/g, ""));
    var subTotal;
    var weight;
    var VAT;

    if ($('#product-vat').val() === 'Không chịu thuế') {
        VAT = 0;
    } else {
        VAT = parseFloat($('#product-vat').val());
    }

    if ($('#product-index').val() == "true") {
        if (newNumber > oldNumber) {
            weight = newNumber - oldNumber;
            subTotal = weight * unitPrice;

        }
        else {
            subTotal = unitPrice;
            weight = 0;
        }
    } else {
        weight = parseInt($('#product-weight').val());
        subTotal = weight * unitPrice;
    }

    var VATAmount = (subTotal * VAT) / 100;
    var total = VATAmount + subTotal;
    $('#product-weight').val(weight);
    $('#product-sub-total').val(numberWithCommas(subTotal));
    $('#product-VATAmount').val(numberWithCommas(VATAmount));
    $('#product-total').val(numberWithCommas(total));

}

//Validate sản phẩm
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
    if ($('#product-time-from').val() === "") {
        $('.time-from-error').html("Vui lòng chọn thời gian bắt đầu.");
        validated = false;
    }
    if ($('#product-time-to').val() === "") {
        $('.time-to-error').html("Vui lòng chọn thời gian kết thúc.");
        validated = false;
    }

    if ($('#product-index').val() == "false") {
        if (parseInt($('#product-weight').val()) < 1) {
            $('.weight-error').html("Vui lòng nhập số lượng và phải lớn hơn 0");
            validated = false;
        }
    }

    if ($('#product-index').val() == "true") {
        if (parseInt($('#product-oldNumber').val()) < 0) {
            $('.oldNumber-error').html("Vui lòng nhập chỉ số cũ");
            validated = false;
        }

        if ($('#product-newNumber').val() == "") {
            $('.newNumber-error').html("Vui lòng nhập chỉ số mới");
            validated = false;
        }
    }
    return validated;
}

//Tính thông báo phí
function calculateTotalAmount() {
    var subTotal = 0;
    $.each(proformaObj, function (index, value) {
        var money = value.SubTotal.replace(/,/g, "");
        subTotal += parseInt(money);
    });

    var vatAmount = 0;
    $.each(proformaObj, function (index, value) {
        var money = value.VATAmount.replace(/,/g, "");
        vatAmount += parseInt(money);
    });

    var totalNoLiabilities = 0;
    $.each(proformaObj, function (index, value) {
        var money = value.Total.replace(/,/g, "");
        totalNoLiabilities += parseInt(money);
    });

    var liabilities = parseInt($('#Liabilities').val().replace(/,/g, ""));
    var total = totalNoLiabilities + liabilities;

    $("#SubTotal").val(numberWithCommas(subTotal));
    $("#TotalNoLiabilities").val(numberWithCommas(totalNoLiabilities));
    $("#VATAmount").val(numberWithCommas(vatAmount));
    $("#Total").val(numberWithCommas(total));
}

//Lấy data Thông báo phí
function getProforma() {
    var customerId = $('#CustomerId').val();
    var data = [];
    var $table = $("#proforma-item-table > tbody");
    var paymentDeadline = $('#PaymentDeadline').val();
    var oldNo;
    var newNo;

    var submitObj = proformaObj.reduce((o, e) => {
        var b = Object.assign(
            {},
            e,
            {
                VATRate: parseFloat(VAT[e.VATRate]),
                UnitPrice: e.UnitPrice.replace(/,/g, ""),
                //SubTotal: e.SubTotal.replace(/,/g, ""),
                //Total: e.Total.replace(/,/g, ""),
                //Total: e.Total.replace(/,/g, ""),
                NewNumber: e.NewNumber == '' ? -1 : parseInt(e.NewNumber),
                OldNumber: e.OldNumber == '' ? -1 : parseInt(e.OldNumber)           
            });
        o.push(b);
        return o;
    }, [])


    var proforma = {
        Deadline: paymentDeadline,
        SubTotal: parseFloat($('#SubTotal').val().replace(/,/g, "")),
        VATAmount: parseFloat($('#VATAmount').val().replace(/,/g, "")),
        TotalNoLiabilities: parseFloat($('#TotalNoLiabilities').val().replace(/,/g, "")),
        Liabilities: parseFloat($('#Liabilities').val().replace(/,/g, "")),
        Total: parseFloat($('#Total').val().replace(/,/g, "")),
        CustomerId: $("#CustomerId").val(),
        CustomerEnterprise: $("#Enterprise").val(),
        ProformaInvoiceItems: submitObj
    };

    return proforma;
}

function renderItemTable() {
    $table = $("#proforma-item-table tbody");
    $table.empty();

    $.each(proformaObj, function (index, value) {
        var template = $('#template-item-table-row').html();
        template = $(template);

        template.find('.name').text(value.ProductName);
        template.find('.index').text(value.HasIndex);
        template.find('.unit').text(value.ProductUnit);
        template.find('.dateFrom').text(value.DateFromS);
        template.find('.dateTo').text(value.DateToS);
        template.find('.oldNum').text(value.OldNumber);
        template.find('.newNum').text(value.NewNumber);
        template.find('.weight').text(value.Quantity);
        template.find('.unit-price').text(value.UnitPrice);
        template.find('.subTotal').text(value.SubTotal);
        template.find('.VATRate').text(value.VATRate);
        template.find('.VATAmount').text(value.VATAmount);
        template.find('.total').text(value.Total);
        template.find('.js-item-update, .js-item-delete').attr('data-selected', value.ProductId);

        $table.append(template[0].outerHTML);
    });
}

$(document).ready(function () {

    toastr.options = {
        progressBar: true,
        showMethod: 'slideDown',
        timeOut: 2300
    };
    //ajaxGetProducts();
    $("#product-oldNumber, #product-newNumber, #product-price, #product-weight").attr('disabled', true);

    $('#product-oldNumber').on('change', function () {
        $('.oldNumber-error').html("");
        var old = parseInt($('#product-oldNumber').val());
        if (old < 0) {
            $('.oldNumber-error').html("Chỉ số mới phải lớn hơn 0");
            $('#product-oldNumber').val("");
            $('#product-oldNumber').focus();
        }
        else {
            $('#product-newNumber').attr({ "min": old + 1 });
        }
    })

    $('#product-oldNumber').on('change', function () {
        productCalculate();
    })

    $('#product-newNumber').on('change', function () {
        $('.newNumber-error').html("");
        var oldNo = parseInt($('#product-oldNumber').val());
        var newNo = parseInt($('#product-newNumber').val());
        if (newNo < 0) {
            $('.newNumber-error').html("Chỉ số mới phải lớn hơn 0.");
            $('#product-newNumber').val("");
            $('#product-newNumber').focus();
        }
        if (newNo <= oldNo) {
            $('.newNumber-error').html("Chỉ số mới phải lớn hơn chỉ số cũ.");
            $('#product-newNumber').val("");
            $('#product-newNumber').focus();
        } else {
            productCalculate();
        }

    })

    $('#product-price').on('change', function () {
        productCalculate();
    })

    $('#product-weight').on('change', function () {
        productCalculate();
    })

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


    //Load product - Combobox
    $('#product-list').on('change', function () {
        ajaxGetOldNumber();

        var data = $(this).find('option:selected').data('opt');
        var weight = 0;
        var unitPrice = parseFloat((data.ProductUnitPrice).replace(/,/g, ""));
        var VAT;
        if (data.ProductVATRate == '-1') {
            VAT = 0;
        } else {
            VAT = parseFloat(data.ProductVATRate);
        }
        var subTotal = unitPrice;
        var VATAmount = subTotal * VAT;
        var total = subTotal + VATAmount;
        clearProductInput();
        $("#product-oldNumber, #product-newNumber, #product-price").attr('disabled', false);
        if (data.HasIndex == true) {
            $(".has-index").show();
            $("#product-weight").attr('disabled', true);
            $("#product-oldNumber, #product-newNumber").attr('disabled', false);
        } else {
            $("#product-weight").attr("disabled", false);
            $(".has-index").hide();
            $("#product-oldNumber, #product-newNumber").attr('disabled', true);
            weight = 1;
        }
        $('#product-list').val(data.ProductId);
        $('#product-unit').val(data.ProductUnit);
        $('#product-price').val(numberWithCommas(unitPrice));
        $('#product-vat').val(VAT_RATE[(data.ProductVATRate).toString()]);
        $('#product-weight').val(weight);
        $('#product-sub-total').val(numberWithCommas(subTotal));
        $('#product-VATAmount').val(numberWithCommas(VATAmount));
        $('#product-total').val(numberWithCommas(total));
        $('#product-index').val(data.HasIndex);

    })

    //Payment Deadline datepicker
    $("#PaymentDeadline").datepicker({
        format: 'dd/mm/yyyy',
        keyboardNavigation: false,
        forceParse: false,
        autoclose: true,
        startDate: new Date()
    });

    //Sản phẩm: Từ, Đến - datepicker
    $("#product-time-from, #product-time-to").datepicker({
        format: 'dd/mm/yyyy',
        keyboardNavigation: false,
        forceParse: false,
        autoclose: true,
    }).attr('readonly', 'readonly').datepicker("setDate", null);

    $("#product-time-from").on('changeDate', function (e) {
        $("#product-time-to").data('datepicker').setStartDate(e.date);
    });


    //Autocomplete
    allowEnterpriseTypeahead();

    //Popup sản phẩm
    $("#choose-products").on("click", function () {
        if ($("#Enterprise").val() === "") {
            toastr.warning("Vui lòng nhập Tên đơn vị.", "Nhắc nhở");
        }
        else if ($('#CustomerId').val() === "") {
            toastr.warning("Khách hàng không tồn tại! Vui lòng kiểm tra lại.", "Nhắc nhở");
        }
        else {
            $("#modal-form").modal();
            clearProductInput();
            ajaxGetProducts();
            $("#save").attr("data-action-type", "create");
        }
    });

    //Lưu sản phẩm
    $("#save").on("click", function () {
        var actionType = $(this).attr("data-action-type");
        var rowIndex = $(this).attr("data-row");

        $(".product-error").html("");
        $(".price-error").html("");
        $(".quantity-error").html("");
        $('.time-to-error').html("");
        $('.time-from-error').html("");
        $('.weight-error').html("");

        if (validateProduct()) {
            var productId = $("#product-list").val();
            var productVAT = $('#product-vat').val();
            var unit = $('#product-unit').val();
            var selectedIndex = $("#product-list").prop("selectedIndex");
            var oldnumber = $('#product-oldNumber').val();
            var newnumber = $('#product-newNumber').val();
            var datefrom = $("#product-time-from").val();
            var dateto = $("#product-time-to").val();
            var quantity = $("#product-weight").val();
            var price = $("#product-price").val();
            var subtotal = $('#product-sub-total').val();
            var VATamount = $('#product-VATAmount').val();
            var total = $('#product-total').val();
            var index = $('#product-index').val();

          
            if (actionType === "create") {
                var item = proformaObj.filter(e => e.ProductId == productId);
                if (item.length) {
                    swal('Sản phẩm đã tồn tại', 'Bạn đã thêm sản phẩm này vào thông báo phí rồi.', 'warning');
                }
                else {
                    var obj = {
                        ProductId: productId,
                        ProductName: $("#product-list option:selected").text(), 
                        ProductUnit: unit,
                        Quantity: quantity,
                        UnitPrice: price,
                        VATRate: productVAT,
                        OldNumber: oldnumber,
                        NewNumber: newnumber,
                        DateFromS: datefrom,
                        DateToS: dateto,
                        SubTotal: subtotal,
                        VATAmount: VATamount,
                        Total: total,
                        HasIndex: index
                    };
                    proformaObj.push(obj);
                }
                renderItemTable();
                clearProductInput();
            }
            else if (actionType === "update") {
                var item = proformaObj.filter(e => e.ProductId == productId);
                if (item.length) {
                    item[0].OldNumber = oldnumber;
                    item[0].NewNumber = newnumber;
                    item[0].DateFromS = datefrom;
                    item[0].DateToS = dateto;
                    item[0].UnitPrice = price;
                    item[0].SubTotal = subtotal;
                    item[0].VATAmount = VATamount;
                    item[0].Total = total;
                    item[0].Quantity = quantity;
                    item[0].HasIndex = index;
                }

                renderItemTable();
                $("#modal-form").modal('toggle');
            }

            calculateTotalAmount();
        }
    });

    //Chọn sp update
    $("#proforma-item-table").on("click", ".js-item-update", function () {
        $("#product-oldNumber, #product-newNumber, #product-price").attr('disabled', false);
        //$item = $(this).closest("tr");
        var productId = $(this).attr('data-selected');
        var item = proformaObj.filter(e => e.ProductId == productId);
        if (item.length) {
            $("#modal-form").modal();
            $("#product-list").val(productId);
            $("#product-list").attr('disabled', true);
            $('#product-unit').val(item[0].ProductUnit);
            $('#product-price').val(item[0].UnitPrice);
            $('#product-vat').val(item[0].VATRate);
            $('#product-weight').val(item[0].Quantity);
            $('#product-sub-total').val(item[0].SubTotal);
            $('#product-VATAmount').val(item[0].VATAmount);
            $('#product-total').val(item[0].Total);
            $('#product-oldNumber').val(item[0].OldNumber);
            $('#product-newNumber').val(item[0].NewNumber);
            $('#product-time-from').val(item[0].DateFromS);
            $('#product-time-to').val(item[0].DateToS);
            $('#product-index').val(item[0].HasIndex);

            $('#save').attr('data-action-type', 'update');
            console.log($("#save").attr("data-action-type"));
        }
        var boolIndex = (item[0].HasIndex === "true");
        if (item[0].HasIndex == "true" || item[0].HasIndex == true) {
            $(".has-index").show();
            $("#product-weight").attr('disabled', true);
            $("#product-oldNumber, #product-newNumber").attr('disabled', false);
        } else {
            $("#product-weight").attr("disabled", false);
            $(".has-index").hide();
            $("#product-oldNumber, #product-newNumber").attr('disabled', true);
        }
    });

    //Xóa sp
    $("#proforma-item-table").on("click", ".js-item-delete", function () {
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
                var productId = $button.attr('data-selected');
                proformaObj = $.grep(proformaObj, function (e) {
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


});