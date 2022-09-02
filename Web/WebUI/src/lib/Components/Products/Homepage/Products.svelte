<script lang="ts">
    import { products } from "../../../Stores/stores";
    import ProductItem from "./ProductItem.svelte";
    import Loader from "../../Common/Loader.svelte";

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
    