<script lang="ts">
  import { shoppingCart, oneTimeProduct} from "../../Stores/stores";
  import CartProduct from "../ShoppingCart/CartProduct.svelte";
  
  const prods = $oneTimeProduct ? [$oneTimeProduct] : $shoppingCart.count ? 
                $shoppingCart.items : [];
  
</script>

<div class="container">
    <ul class="list">
      {#each prods as prod}
        <li class="element">
          <CartProduct prod={prod} simplified size={60}/>
        </li>
      {:else}
        <h1>Add some products to the cart at first</h1>
      {/each}
    </ul>
</div>

<style>
  li {
    width: 95%;
    display: flex;
    justify-content: center;
    align-items: center;
  }
  .list {
    width: 80%;
    height: 100%;
    border-radius: 2rem;
    border: 0.2rem solid rgb(255, 255, 255);
    background-color: rgba(202, 202, 202, 0.15);
    list-style-type: none;
    display: flex;
    flex-direction: column;
    align-items: center;
    padding: 1.5rem 0;
    overflow: scroll;
  }
  .container {
    margin: 0 2rem;
    width: calc(50vw - 2rem);
    height: 100%;
    display: flex;
    justify-content: center;
    align-items: center;
  }
</style>