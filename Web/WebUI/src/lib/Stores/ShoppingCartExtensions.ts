import { get, writable } from "svelte/store";
import type { CartItem } from "../Models/cart";
import type Cart from "../Models/cart";
import { agent } from "../Utils/agent";
import { products, shoppingCart, userProfile } from "./stores";

export const modifyCart = async (item: CartItem, localChanges: boolean = false) => {
    if(get(userProfile)?.username && !localChanges){
        await agent.ShoppingCart.updateCart(item);
    }
    shoppingCart.update((v) => {
        let itemInCart = v.items.find(x => x.id === item.id);
        if(itemInCart){
            itemInCart.quantity += item.quantity;
        } else {
            v.items.push(item);
        }
        //if quantity < 0 then remove element
        if((itemInCart ?? item).quantity <= 0){
            console.log(v.items);
            v.items = v.items.filter(x => x.id !== item.id);
            console.log(v.items);
        }

        v.count = v.items.reduce((acc, items) => acc+items.quantity, 0);

        v.sum = parseFloat(v.items.reduce((acc, items) => {
            return acc+(get(products).find(x => x.id === items.id)?.price*items.quantity);
        }, 0).toFixed(2));

        return v;
    });
    localStorage.setItem("cart", JSON.stringify(get(shoppingCart)));
}

export const initShoppingCart = (cart?: Cart) => {
    if(cart){
        shoppingCart.set(cart);
    } else {
    shoppingCart.set({items: new Array<CartItem>, count: 0, sum: 0});
    }
    localStorage.setItem("cart", JSON.stringify(get(shoppingCart)));
}

export async function removeShoppingCart() {
    if(get(userProfile)?.username){
        await agent.ShoppingCart.clearCart();
    }
    localStorage.removeItem("cart");
    initShoppingCart();
}

export async function saveLocalCart() {
    const cart: Cart = get(shoppingCart);
    await agent.ShoppingCart.setCart(cart);
}