<script lang="ts">
  import { agent } from '../../Utils/agent';
  import {products} from '../../Stores/stores';
  import ImageModal from "./ImageModal.svelte";
  import Loader from '../Common/Loader.svelte';
  import EditModal from './EditModal.svelte';

  let src = "";
  let loading = ""; //equals id of product which button will be loading
  let edit = "";

  const imageModal = (newSrc: string):any => {
    src = newSrc;
    
  }

  const onEdit = (e: MouseEvent, id: string) => {
    edit = id;
  }

  $: if(src || edit) document.body.style.overflow = 'hidden'; else document.body.style.overflow = 'scroll'; 

  const onRemove = async (id: string) => {
    loading = id;
    await agent.Products.remove(id);
    products.set($products.filter(x => x.id !== id));
    loading = "";
  }

</script>

{#if src!==""}
  <ImageModal bind:src/>
{/if}

{#if edit!==""}
  <EditModal bind:edit />
{/if}

<div class="table">

    <div class="headers">
      {#each Object.keys($products[0]) as key}
      <div class="header">{key}</div>
      {/each}
    </div>
      {#each $products as prod}
      <div class="row">
      {#each Object.keys($products[0]) as key}
          <div class="data">
          {#if prod[key] === prod.id}
            <p>{prod.id}</p>
            <div class="buttons">
              <button class="edit" on:click={(e) => onEdit(e, prod.id)}>Edit</button>
              <button class="remove" on:click={() => onRemove(prod.id)}>
                {#if loading === prod.id}
                  <Loader size={1} inElement color='white'/>
                {:else}
                Remove
                {/if}
              </button>
            </div>
          {:else if prod[key] === prod.description}
            {prod.description.slice(0, 30)+'...'}
          {:else if prod[key] === prod.price}
            {prod.price}$
          {:else if prod[key] === prod.photos}
          {#each prod[key] as photo}
            <img src={photo} alt="product minature" on:click={() => imageModal(photo)}/>
          {/each}
          {:else}
            {prod[key]}
          {/if}
          </div>
      {/each}
      </div>
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
    background-color: white;
    padding: 0;
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
    position: relative;
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