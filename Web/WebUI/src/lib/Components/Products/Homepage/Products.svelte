<script lang="ts">
    import { products } from "../../../Stores/stores";
    import ProductItem from "./ProductItem.svelte";
    import Loader from "../../Common/Loader.svelte";
    //TODO: page automatically scrolls back up on page change, fix it
</script>
    {#if !$products}
        <Loader entire />
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
    