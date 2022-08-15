<script lang="ts">
    import ProductItem from "./ProductItem.svelte";
    
    
    
        async function getProducts() {
            const res = await fetch("http://localhost:5000/api/products")
    
            if(!res.ok)
                throw new Error(await res.text());
    
            const products = res.json();
            return products;
        };
    
        const products = getProducts();
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
    