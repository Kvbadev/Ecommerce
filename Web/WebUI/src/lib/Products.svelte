<script lang="ts">
import ProductItem from "./ProductItem.svelte";



    async function getProducts() {
        const res = await fetch("http://localhost:5000/api/products")

        let products;
        if(res.ok){
            products = res.json();
        } else { 
            throw new Error(await res.text());
        }

        console.log(products);
        return products
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
