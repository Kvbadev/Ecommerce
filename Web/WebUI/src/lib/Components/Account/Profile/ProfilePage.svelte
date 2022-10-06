<script lang="ts">

import { push } from "svelte-spa-router";

import { userProfile } from "../../../Stores/stores";
import Loader from "../../Common/Loader.svelte";
import ProfileInfo from "./ProfileInfo.svelte";
import TransactionsInfo from "./TransactionsInfo.svelte";



if(!localStorage.getItem("jwt")){
    push('/account/login');
}
</script>


{#if !$userProfile}
    <Loader />
{:else}
<div class="container">
    <ProfileInfo />
    <TransactionsInfo />
</div>
{/if}

<style>
.container {
    width: 100vw;
    height: calc(100vh - 4.2vw);
    display: grid;
    grid-template-columns: 1fr 1fr;
    grid-template-rows: 1fr ;
}
</style>