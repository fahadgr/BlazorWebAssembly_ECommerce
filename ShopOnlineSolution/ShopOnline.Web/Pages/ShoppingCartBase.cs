using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Net;

namespace ShopOnline.Web.Pages
{
    public class ShoppingCartBase : ComponentBase
    {
        [Inject]
        public IJSRuntime JSRuntime { get; set; }

        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        public List<CartItemDto> ShoppingCartItems { get; set; }
        public string ErrorMessage { get; set; } 

        protected string TotalPrice { get; set; }
        protected int TotalQty { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                CalculateCartSummaryTotals();
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        protected async Task UpdateQty_Input(int id)
        {
            await QtyButtonVisibility(id, true);
        }

        protected async Task QtyButtonVisibility(int id,bool visible)
        {
            await JSRuntime.InvokeVoidAsync("UpdateQty_Visible", id, visible);
        }

        protected async Task DeleteCartItem_Click(int id)
        {
            var cartItemDto = await ShoppingCartService.DeleteItem(id);
            RemoveCartItem(id);
            CalculateCartSummaryTotals();
        }

        private void UpdateItemTotalPrice(CartItemDto cartItemDto)
        {
            var item = GetCartItem(cartItemDto.Id);
            if (item != null)
            {
                item.TotalPrice = cartItemDto.Price * cartItemDto.Qty;
            }
        }

        private void CalculateCartSummaryTotals()
        {
            SetTotalPrice();
            SetTotalQuantity();
        }

        private void SetTotalPrice()
        {
            TotalPrice = ShoppingCartItems.Sum(x => x.TotalPrice).ToString("C");
        }

        private void SetTotalQuantity()
        {
            TotalQty = ShoppingCartItems.Sum(x => x.Qty);
        }


        private CartItemDto GetCartItem(int id)
        {
            return ShoppingCartItems.FirstOrDefault(x => x.Id == id);
        }



        protected async Task UpdateQtyCartItem_Click(int id,int qty)
        {
            try
            {
                if (qty > 0)
                {
                    var updateItemDto = new CartItemQtyUpdateDto
                    {
                        CartItemId = id,
                        Qty = qty
                    };

                    var returnedUpdateItemDto = await ShoppingCartService.UpdateQty(updateItemDto);

                    UpdateItemTotalPrice(returnedUpdateItemDto);
                    CalculateCartSummaryTotals();
                    await QtyButtonVisibility(id, false);
                }
                else
                {
                    var item = ShoppingCartItems.FirstOrDefault(i=>i.Id == id);
                    if (item != null)
                    {
                        item.Qty = 1;
                        item.TotalPrice = item.Price;
                    }
                }
                
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void RemoveCartItem(int id)
        {
            var CartItemDto = GetCartItem(id);
            ShoppingCartItems.Remove(CartItemDto);
        }
    }
}
