<script lang="ts">
    import { agent } from "../../../Utils/agent";
    import ProductItem from "./ProductItem.svelte";

    </script>
    
    <ul class="list">
            {#await agent.Products.get()}
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
        ul {
            display: flex;
            flex-direction: row;
            flex-wrap: wrap;
        }
    </style>
    