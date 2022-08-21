<script lang="ts">
    import { onMount, onDestroy } from "svelte";
    import { products } from "../../../Stores/stores";
    import type Product from "src/lib/Models/product";
    import { agent } from "../../../Utils/agent";
    import ProductItem from "./ProductItem.svelte";

    let prods: Array<Product>;

    const unsubscribe = products.subscribe(value => {
        prods = value
    })

    async function getProducts() {
        products.set(await agent.Products.get())
    }

    onMount(async () => {
        getProducts();
    });

    onDestroy(unsubscribe);

</script>
    
    <ul class="list">
        {#each prods as product}
            <ProductItem product={product}/>
        {/each}
    </ul>
    
    <style>
        ul {
            display: flex;
            flex-direction: row;
            flex-wrap: wrap;
            justify-content: space-evenly;
        }
    </style>
    