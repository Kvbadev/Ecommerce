<script lang="ts">
  import { shoppingCart, products} from "../../Stores/stores";

  const prods = $shoppingCart?.items.map(v => $products.find(x => x.id == v.id))

</script>

<div class="container">
    <ul class="list">
        {#each prods as prod}
          <li class="element">{prod.name}</li>
        {/each}
    </ul>
</div>

<style>
  .container {
    width: 50vw;
    height: 100%;
  }
  ul {
    list-style-type: none;
  }
</style>