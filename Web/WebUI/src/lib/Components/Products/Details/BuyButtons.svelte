<script lang="ts">
import { updateShoppingCart } from "../../../Stores/shoppingCartStore";


export let params = {} as any;

export let quantity: number;
export let price: number;

const OnCart = () => {
    updateShoppingCart({quantity: quantity, id: params.id, price: price});
}

</script>

<div class="container">
    
    <button class="buy-btn"><span>Buy Now</span></button>
    <button on:click={OnCart} class="cart-btn"><span>Add to cart</span></button>
</div>

<style>
    button {
        cursor: pointer;
        font-family: Raleway;
        border: black 0.4rem solid;
        width: 33rem;
        height: 9rem;
        background-color: transparent;
        font-size: 3rem;
        text-align: center;
        transition: position 0.5s ease-in-out;
    }
    button:active {
        background-color: white;
    }
    span::after {
        content: '»'; 
        position: absolute;
        right: -2rem;
        top: 0;
        /* left: 1.5rem; */
        opacity: 0;
        transition: 0.5s;
    }
    button:hover span:after {
        opacity: 1;
        right: 0;
    }
    span {
        display: inline-block;
        position: relative;
        transition: 0.5s; 
    }
    button:hover span{
        padding-right: 2.5rem;
    }
    div {
        display: flex;
        align-items: center;
        justify-content: space-around;
    }
    .cart-btn {
        background-color: black;
    }
    .cart-btn span{
        color: white;
    }

</style>