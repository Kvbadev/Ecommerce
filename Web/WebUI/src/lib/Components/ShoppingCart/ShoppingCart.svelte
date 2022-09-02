<script lang="ts">

import {shoppingCart, removeShoppingCart} from '../../Stores/shoppingCartStore';

import {products} from '../../Stores/stores';

import Loader from '../Common/Loader.svelte';

function clearCart() {
   removeShoppingCart(); 
}


</script>

{#if !$shoppingCart || !$products}
    <Loader />
{:else}
    {#if $shoppingCart.items.length}
        <h1>{$shoppingCart.sum}$</h1>
        {#each $shoppingCart.items.map(v => {
            const item = $products.find(x => x.id===v.id);
            const sum = v.price * v.quantity;
            return {item: item, quantity: v.quantity, sum: sum}
            }) as {item, quantity, sum}}

            <h3>{item?.name} [{quantity}] - {sum}$</h3>
        {/each}
        <button on:click={clearCart}>Clear the cart</button>
    {:else}
        <h1>You have not added any products to your cart</h1>
    {/if}
{/if}