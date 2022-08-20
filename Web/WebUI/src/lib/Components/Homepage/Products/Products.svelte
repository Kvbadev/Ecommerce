<script lang="ts">

    import { agent } from "../../../Utils/agent";
import ProductItem from "./ProductItem.svelte";

    const products = agent.Products.get();
    </script>
    
    <ul class="list">
            {#await products}
                waiting...
            {:then data}
                {#each data as product}
                    <ProductItem product={product}/>
                {/each}
            {:catch error}
                <p style="color: red">{error.message}</p>
            {/await}
    </ul>
    
    <style>
        .list {
            display: grid;
            grid-template-columns: auto auto;
        }
    </style>
    