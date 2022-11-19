<script lang="ts">
    import { initProducts, products } from "../../../Stores/stores";
    import ProductItem from "./ProductItem.svelte";
    import Loader from "../../Common/Loader.svelte";
    import { onMount } from "svelte";

    onMount(async () => {
        if(!$products) await initProducts();
    })
</script>
    {#if !$products}
        <Loader />
    {:else}
    <ul class="list">
        {#each $products as product}
            <ProductItem product={product}/>
        {/each}
    </ul>
    {/if}
    
    <style>
        ul {
            display: flex;
            flex-direction: row;
            flex-wrap: wrap;
            justify-content: space-evenly;
        }
    </style>
    