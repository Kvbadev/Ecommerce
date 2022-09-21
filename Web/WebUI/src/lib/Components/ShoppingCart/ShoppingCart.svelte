<script lang="ts">

import {removeShoppingCart, saveLocalCart} from '../../Stores/ShoppingCartExtensions';

import {products, userProfile, shoppingCart} from '../../Stores/stores';

import Loader from '../Common/Loader.svelte';
import { link } from 'svelte-spa-router';
import CartProduct from './CartProduct.svelte';
  import { onDestroy, onMount } from 'svelte';
  import type Cart from 'src/lib/Models/cart';
  import { get } from 'svelte/store';

async function clearCart() {
   removeShoppingCart(); 
}

let changes: Cart;

onMount(() => {
    changes = {
        ...get(shoppingCart)
    }
})

onDestroy(async () => {
    if(JSON.stringify(changes) !== JSON.stringify($shoppingCart) && 
       $shoppingCart.items.length !== null){
        await saveLocalCart();
    }
})

</script>

<div class="all">

{#if !$shoppingCart || !$products}
    <Loader entire/>
{:else}

<h1>My Shopping Cart</h1>
<div class="container">
    {#if $shoppingCart.items.length}
        <div class="items">
            {#each $shoppingCart.items as cartitem}
                <CartProduct prod={cartitem} />
            {/each}
        </div>
        {#if $userProfile}
        <a href="/Buy" use:link><button>Buy products</button></a> 
        {:else}
        <a href="/Account/Login" use:link><button >Buy products</button></a>
        {/if}

        <button on:click={clearCart}>Clear the cart</button>
    {:else}
        <h1>You have not added any products to your cart</h1>
    {/if}
</div>
{/if}
</div>

<style>
    .all {
        width: 100%;
        height: calc(100vh - 4.2vw);
    }
    .items {
        display: flex;
        justify-content: center;
        align-items: center;
        flex-direction: column;
        margin: 2rem 0;
    }
    h1 {
        padding: 3rem;
        font-family: 'Raleway';
        text-align: center;
    }
    .container {
        overflow: auto;
        margin: 0 auto;
        width: 90%;
        height: 80%;
        border: 0.5rem black solid;
        border-radius: 1rem;
    }
    a {
        width: auto;
        display: inline-block;
    }
    button {
        display: block;
        width: 15rem;
        border: none;
        background-color: rgb(184, 184, 184);
        height: 5rem;
        cursor: pointer;
    }
</style>