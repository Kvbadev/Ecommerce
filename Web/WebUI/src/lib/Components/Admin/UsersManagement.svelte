<script lang="ts">
  import { agent } from "../../Utils/agent";
  import { onMount } from "svelte";
  import Loader from "../Common/Loader.svelte";
  import type Client from "src/lib/Models/client";
  import Fa from "svelte-fa";
  import { faHouseMedicalCircleXmark, faMehRollingEyes, faRemove, faSackXmark, faUserEdit, faUserLock, faXmark, faXmarkCircle, faXmarksLines } from "@fortawesome/free-solid-svg-icons";
  import { xlink_attr } from "svelte/internal";


let loading = false, clients: Client[] = null;
const removeRole = async (cl: Client, role: string) => {
  await agent.Admin.updateRoles(cl.privileges.filter(x => x !== role), cl);
  clients = [...clients.filter(x => x.username!=cl.username),{...cl, privileges: cl.privileges.filter(x => x !== role)}]
}


onMount(async () => {
    clients = await agent.Admin.getClients();
    for(const cl of clients)
      cl.createdAt = cl.createdAt.split('T')[0];
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
        {#if key === 'privileges'}
        {#each cl[key] as role}
          <span on:click={() => removeRole(cl,role)}><p>{role}</p><Fa icon={faXmark} color="#ff0000" size="1.1x"/></span>
        {/each}
        {:else}
        {cl[key]}{#if key == 'moneySpent'}$
        {/if}
        {/if}
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
  .data span {
    margin-left: 1rem;
    display: flex;
    flex-direction: row;
    justify-content: space-evenly;
    align-items: flex-end;
    cursor: pointer;
  }
  .data > span > p {
    margin-right: 0.4rem;
  }
  .data {
    font-size: 2.5rem;
    width: calc(100% / 4);
    height: 8rem;
    display: flex;
    flex-direction: row;
    border-right: 0.1rem solid black;
    background-color: white;
    padding: 0;
    justify-content: center;
    align-items: center;
    font-family: 'Rubik';
    overflow: scroll;
  }
</style>