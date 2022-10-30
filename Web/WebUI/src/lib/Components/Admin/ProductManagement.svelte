<script lang="ts">
  import { prop_dev } from "svelte/internal";
  import type Product from "../../Models/product";
  import {products} from '../../Stores/stores';
  import ImageModal from "./ImageModal.svelte";

  let src = "";

  const imageModal = (newSrc: string):any => {
    src = newSrc;
  }

</script>
{#if src!==""}
  <ImageModal bind:src/>
{/if}

<div class="table">

    <div class="headers">
      {#each Object.keys($products[0]).filter(x => x!=='mainPhoto') as key}
      <div class="header">{key}</div>
      {/each}
    </div>
      {#each $products as prod}
      <div class="row">
      {#each Object.keys($products[0]).filter(x => x!=='mainPhoto') as key}
          <div class="data">
          {#if prod[key] === prod.id}
            <p>{prod.id}</p>
            <div class="buttons">
              <button class="edit">Edit</button>
              <button class="remove">Remove</button>
            </div>
          {:else if prod[key] === prod.description}
            {prod.description.slice(0, 30)+'...'}
          {:else if prod[key] === prod.price}
            {prod.price}$
          {:else if prod[key] === prod.photos || prod[key] === prod.mainPhoto}
          {#each [...prod[key], prod.mainPhoto] as photo}
            <img src={photo} alt="product minature" on:click={() => imageModal(photo)}/>
          {/each}
          {:else}
            {prod[key]}
          {/if}
          </div>
      {/each}
      </div>
      <!-- <div class="buttons">
        <button class="edit">Edit</button>
        <button class="remove">Remove</button>
      </div> -->
      {/each}
</div>

<style>
 
  .data img{
    border-radius: 1rem;
    cursor: pointer;
  }
  .table {
    display: flex;
    flex-direction: column;
  }
  .headers {
    width: 92vw;
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    align-items: center;
    font-size: 2.7rem;
    font-family: 'Rubik';
    background-color: rgba(0, 0, 0, 0.2);
    height: 5rem;
  }
  .header {
    height: 100%;
    border-right: 0.1rem solid black;
    display: grid;
    place-items: center;
    width: calc(100% / 5);
  }
  .row {
    width: 92vw;
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    align-items: center;
    border-bottom: 0.1rem solid black;
  }
  .data:first-child {
    font-size: 1.4rem;
    display: flex;
    flex-direction: column;
    justify-content: space-evenly;
    align-items: center
  } 
  .data {
    font-size: 1.8rem;
    width: calc(100% / 5);
    height: 8rem;
    display: flex;
    border-right: 0.1rem solid black;
    justify-content: center;
    align-items: center;
    font-family: 'Rubik';
    overflow: scroll;
  }
  .data img {
    width: 5rem;
    height: 5rem;
    margin: 0 0.5rem
  }
  .buttons {
    display: flex;
    flex-direction: row;
    justify-content: flex-end;
  }
  .buttons * {
    cursor: pointer;
    border-top-right-radius: 0.7rem;
    border-bottom-left-radius: 0.7rem;
    margin: 0 0.5rem;
    border: none;
    width: 10rem;
    height: 2rem;
    color: white;
    background-color: red;
    transition: 0.3s ease-in;
  }
  .buttons *:hover {
    box-shadow: rgba(0, 0, 0, 0.20) 0px 5px 15px;
  }
  .edit {
    background-color:rgb(255, 205, 139);
  }

</style>