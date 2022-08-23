<script lang="ts">
    import { onMount, onDestroy } from "svelte";
    import { products } from "../../../Stores/stores";
    import type Product from "src/lib/Models/product";
    import { agent } from "../../../Utils/agent";
    import ProductItem from "./ProductItem.svelte";
import Loader from "../../Common/Loader.svelte";

    let prods: Array<Product>;

    const unsubscribe = products.subscribe(value => {
        prods = value
    })

    async function getProducts() {
        // setTimeout(async () => (products.set(await agent.Products.get())), 1000);
        products.set(await agent.Products.getAll());
    }

    onMount(async () => {
        getProducts();
    });

    onDestroy(unsubscribe);

</script>
    {#if !prods.length}
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
    