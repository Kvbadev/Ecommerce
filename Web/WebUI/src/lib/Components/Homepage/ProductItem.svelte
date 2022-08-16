<script lang="ts">
    export let product:any;
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