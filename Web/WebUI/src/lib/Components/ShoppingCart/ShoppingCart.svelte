<script lang="ts">
import { onMount } from 'svelte';

import {shoppingCart, products, getProducts} from '../../Stores/stores'

let items;
onMount(async () => {if(!$products){
    await getProducts();
    console.log($shoppingCart.items)
    items = $shoppingCart.items.map((v) => {
        return $products.find(x => x.id === v.id); 
    }).filter(x => x !== undefined);
    console.log(items);
}
});


</script>

{#if $shoppingCart && items}
<h1>{$shoppingCart.sum}$</h1>
{#each items as item}
    <h3>{item.name}x</h3>
{/each}

{:else}
<h1>You've added no items to your cart</h1>
{/if}