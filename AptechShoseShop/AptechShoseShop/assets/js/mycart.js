$.cookie.json = true;
$.cookie.defaults.path = '/';
//var arrAjax = new Object();

function getCartItems() {
    if ($.cookie('productlist')) {
        return $.cookie('productlist').cartItems;
    } else {
        return [];
    }
}


function saveCartItems(cart_items) {

    var obj = { "cartItems": cart_items };
    $.cookie('productlist', obj, { expires: 30 }); //expires = 30 days
}
function emptyCartItems() {
    $.removeCookie('productlist');
}



function addCart(productId) {
    $(document).ready(function () {   
        var colorName = document.getElementById("ColorName");
        var colorSelected = colorName.options[colorName.selectedIndex].value;
        if (colorSelected === "#") {
            alert("Bạn chưa chọn màu");
            return;
        };

        var sizeName = document.getElementById("SizeName");
        var sizeSelected = sizeName.options[sizeName.selectedIndex].value;
        if (sizeSelected === "#") {
            alert("Bạn chưa chọn size");
            return;
        };

        var quantity = $("#quantity").val();      
        var cart_items = getCartItems();
      
        var is_exist = false;
        $(cart_items).each(function (i, v) {
            if (v && v.productid === productId && v.ColorName === colorSelected && v.SizeName === sizeSelected) {
                is_exist = true;
            }
        });
        
        if (!is_exist) {
            var new_item = {
                "productid": productId,
                "quantity": quantity,
                "ColorName": colorSelected,
                "SizeName": sizeSelected
            };
            cart_items.push(new_item);
            saveCartItems(cart_items);
            swal({
                text: "Bạn đã thêm sản phẩm này vào giỏ hàng.",
                icon: "success",
                buttons: {
                    cancel: "Tiếp tục mua hàng",
                    catch: {
                        text: "Đi đến thanh toán",
                        value: "cart"
                    }
                }
            }).then((value) => {
                switch (value) {
                    case "cart":
                        location.href = "/cart";
                        break;
                }
            });
        }
        else {

            var cart_items = getCartItems();
            let itemCart = cart_items.find(x => x.productid === productId && x.SizeName === sizeSelected && x.ColorName === colorSelected);
            var quantityCart = parseInt(itemCart["quantity"]) + 1;
            var colorCart = itemCart["ColorName"];
            var sizeCart = itemCart["SizeName"];
            updateItem(productId, quantityCart, colorCart, sizeCart);                      
            swal("Notification", "Bạn đã thêm giỏ hàng " + quantityCart + " sản phẩm này.");
          
        }
       
    });
   
 
}


function bindingCart(type) {
    $(document).ready(function () {
        var cart_items = getCartItems();
        $.ajax({
            type: "GET",
            url: '/Cart/Binding',
            data: "data=" + JSON.stringify(cart_items) + "&type=" + type,
            success: function (response) {
                $("#" + type).html(response);


            },
            error: function (ex) {

                console.log("Error!!" + ex);
            }

        });


    });
}


function updateItem(productid, q, color, size) {
    productid = productid.toString();
    q = q.toString();
    color = color.toString();
    size = size.toString();
    /*console.log();*/
    var cart_items = getCartItems();

    $(cart_items).each(function (i, v) {
        if (v && v.productid === productid && v.ColorName === color && v.SizeName === size) {
            cart_items[i].quantity = q;
            saveCartItems(cart_items);
            bindingCartCommon();
        }
    });

}



function bindingCartCommon() {
    bindingCart("binding-cart");
    bindingCart("small-cart-binding");
}


function removeItem(id) {
    swal({
        title: "Xác nhận xóa.",
        text: "Xóa sản phẩm này khỏi giỏ hàng",
        icon: "warning",
        buttons: true,
        dangerMode: true
    })
        .then((isConfirm) => {
            if (isConfirm) {
                //  id = id.toString();
                var cart_items = getCartItems();
                for (var i = 0; i < cart_items.length; i++) {
                    if (cart_items[i].productid === id) {

                        cart_items.splice(i, 1);
                    }
                }
                saveCartItems(cart_items);
                bindingCartCommon();

            } else {
                // nếu ấn cancel
            }
        });

}




function Checkout() {
    $(document).ready(function () {

        var cart_items = getCartItems();
        var CustomerName = $("#fullName").val();
        var CustomerPhone = $("#phone").val();
        var CustomerAddress = $("#address").val();
        var CustomerEmail = $("#email").val();
        var OrderNote = $("#orderNote").val();
      

        $.ajax({
            url: '/Checkout/Checkout',
            type: "POST",

            data: "data=" + JSON.stringify(cart_items) + "&CustomerName=" + CustomerName
                + "&CustomerPhone=" + CustomerPhone
                + "&CustomerEmail=" + CustomerEmail
                + "&CustomerAddress=" + CustomerAddress
            
                + "&OrderNote=" + OrderNote
          ,
            success: function (response) {
                if (response === "OK") {
                    emptyCartItems();
                    swal(
                        "Thông báo !",
                        {
                        text: "Bạn đã đặt hàng thành công.",
                        icon: "success",
                        buttons: {
                            catch: {
                                text: "OK",
                                value: "Product"
                            }
                        }
                        }).then((value) => {
                            switch (value) {
                                case "Product":
                                    location.href = "/products";
                                    break;
                            }
                        });
                }
                else {
                    //alert(response);
                    swal(response);
                }
            },
            error: function () {
                console.log("Lỗi");
            }
        });
    });

}
//





