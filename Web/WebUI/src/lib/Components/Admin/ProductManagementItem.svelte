<script lang="ts">
  import type Product from "../../Models/product";
  import CartProduct from "../ShoppingCart/CartProduct.svelte";


    export let product:Product;
</script>

<li>
    <CartProduct prod={{...product, quantity:2}}/>
</li>

<style>
    li {
        width: 80%;
        display: grid;
        place-items: center;
    }
</style>