function UpdateQty_Visible(id, visible)
{
    var updateQtyButton = document.querySelector("button[data-itemId='" + id + "']");

    if (visible == true) {
        updateQtyButton.style.display = "inline-block";
    } else {
        updateQtyButton.style.display = "none";    
    }
}