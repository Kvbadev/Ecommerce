<script lang="ts">
  import { agent } from "../../Utils/agent";
  import { onMount } from "svelte";
  import Loader from "../Common/Loader.svelte";
  import type Client from "src/lib/Models/client";


let loading = false, clients: Client[] = null;
const onEdit = (e: Event) => {

}


onMount(async () => {
    clients = await agent.Account.getClients();
    for(const cl of clients)
    {
      cl.createdAt = cl.createdAt.split('T')[0];
    }
})
</script>

<div class="container">
  {#if clients}
    <div class="headers">
      {#each Object.keys(clients[0]) as key}
        <div class="header {key}">
          {#if key === 'moneySpent'}
          Money Spent
          {:else if key === 'createdAt'}
          Created At
          {:else}
          {key}
          {/if}
        </div>
      {/each}
    </div>

    {#each clients as cl}
    <div class="row">
      {#each Object.keys(clients[0]) as key}
      <div class="data">
        {cl[key]}{#if key == 'moneySpent'}${/if}
      </div>
      {/each}
    </div>
    {/each}

    {:else}
    <Loader />
    {/if}
</div>

<style>
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
    width: calc(100% / 4);
  }

  .row {
    width: 92vw;
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    align-items: center;
    border-bottom: 0.1rem solid black;
  }

  .data {
    font-size: 2.5rem;
    width: calc(100% / 4);
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
</style>