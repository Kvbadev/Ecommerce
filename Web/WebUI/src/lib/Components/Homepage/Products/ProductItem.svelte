<script lang="ts">
    import type Product from "src/lib/Models/product";

    export let product:Product;
</script>

<li name={product.id}>
    <p>{product.name}</p>
    <p>{product.description}</p>
    <p>{product.price}</p>
</li>

<style>
    li {
        font-size: 20px;
        width: 50%;
        padding: 10px;
        margin: 20px;
    }
</style>