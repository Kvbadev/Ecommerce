<script lang="ts">
  import { agent } from "../../../Utils/agent";
  import type Transaction from "../../../Models/Transaction";
import { onMount } from "svelte";
  import Loader from "../../Common/Loader.svelte";

    onMount(async () => {
    })
</script>

<div class="container">
    <ul class="list">
        {#await agent.Account.getTransactions()}
            <Loader inElement />
        {:then value} 
        {#each value as t}
        <li>
            {t.products}~{t.price}
        </li>
        {/each}
        {/await}
    </ul>
</div>

<style>
    .container {
        background-color: beige;
        width: 100%;
        height: calc(100vh - 4.2vw);
        overflow: auto;
    }
</style>