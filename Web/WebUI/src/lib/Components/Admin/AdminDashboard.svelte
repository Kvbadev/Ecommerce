<script lang="ts">
  import { faBox, faSignOut, faUser } from "@fortawesome/free-solid-svg-icons";
  import SideNav from "./SideNav.svelte";
  import {products} from '../../Stores/stores';
  import ProductManagementItem from './ProductManagementItem.svelte';



  let page:0|1 = 0; //products 

</script>

<div class="container">
<SideNav bind:page num={2} icons={[faBox, faUser, faSignOut]}/>

  <table>
    {#if page === 0 && $products}
    <thead>
      {#each Object.keys($products[0]).filter(x => x!=='mainPhoto') as key}
        <th>{key}</th>
      {/each}
    </thead>
    <tbody>
      {#each Object.keys($products[0]).filter(x => x!=='mainPhoto') as key}
      <tr>
        {#each $products as prod}
        <td>
          {prod[key]}
        </td>
        {/each}
      </tr>
      {/each}
    </tbody>
    {/if}
  </table>



  <!-- {:else} -->
    <!-- {#each $products as product}
      <ProductManagementItem product={product}/>
    {/each} -->
  <!-- {/if} -->
</div>

<style>
  .container {
    display: flex;
    flex-direction: row;
    width: 100vw;
    height: 100vh;
  } 
  table {
    width: 92vw;
    list-style: none;
    overflow: scroll;
  }
  th {
    border: 0.1rem black solid;
    margin: 0.5rem;
    font-size: 2rem;
    font-family: 'Raleway';
    width: calc(100%/5);
  }
  tr {
    width: calc(100%/5);
    overflow: hidden;
    display: flex;
    flex-direction: column;
    align-items: center;
  }
  table > * {
    display: flex;
    flex-direction: row;
  }
</style>