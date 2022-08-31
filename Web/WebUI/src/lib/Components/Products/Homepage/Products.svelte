<script lang="ts">
    import { onMount, onDestroy } from "svelte";
    import { products, getProducts } from "../../../Stores/stores";
    import type Product from "src/lib/Models/product";
    import ProductItem from "./ProductItem.svelte";
import Loader from "../../Common/Loader.svelte";

    let prods: Array<Product>;

    const unsubscribe = products.subscribe(value => {
        prods = value
    })



    onMount(async () => {
        getProducts();
    });

    onDestroy(unsubscribe);

</script>
    {#if !prods}
        <Loader />
    {:else}
    <ul class="list">
        {#each prods as product}
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
    