<script lang="ts">

import {removeShoppingCart, removeFromCart} from '../../Stores/ShoppingCartExtensions';

import {products, userProfile, shoppingCart} from '../../Stores/stores';

import Loader from '../Common/Loader.svelte';
import { link } from 'svelte-spa-router';
import { faMinus} from '@fortawesome/free-solid-svg-icons';
import Fa from 'svelte-fa';
import type { CartItem } from 'src/lib/Models/cart';

async function clearCart() {
   removeShoppingCart(); 
}
function removeItem(item: CartItem){
    removeFromCart(item);
}


</script>

<div class="all">

{#if !$shoppingCart || !$products}
    <Loader entire/>
{:else}

<div class="container">
    {#if $shoppingCart.items.length}
        <h1>{$shoppingCart.sum.toFixed(2)}$</h1>
        {#each $shoppingCart.items.map(v => {
            const item = $products.find(x => x.id===v.id);
            const sum = item.price * v.quantity;
            return {item: item, quantity: v.quantity, sum: sum}
            }) as {item, quantity, sum}}

            {#if quantity}
            <div class="item">
                <h3>{item?.name} [{quantity}] - {sum.toFixed(2)}$</h3>
                <span class="remove-item" on:click={() => removeItem({id: item.id, quantity: 1})}>
                    <Fa icon={faMinus} size='lg'/>
                </span>
            </div>  
            {/if}
        {/each}

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
    .container *{
        margin: 0.1rem 1rem ;
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
    h3 {
        display: inline-block;
        margin: 0;
        padding: 0;
    }
    .item {
        /* width: 32rem; */
        display: flex;
        flex-direction: row;
        align-items: center;
        justify-content: flex-start;
    }
    .remove-item {
        cursor: pointer;
        margin: 0;
        padding: 0;
        width: auto;
        height: auto;
    }
</style>