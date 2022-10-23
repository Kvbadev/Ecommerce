<script lang="ts">
  import { agent } from '../../../Utils/agent';
import { push } from 'svelte-spa-router';
import { initShoppingCart } from '../../../Stores/ShoppingCartExtensions';
import {jwtToken, refreshToken, userProfile} from '../../../Stores/stores'
import Loader from '../../Common/Loader.svelte';

let clientInfo = {
    firstname: $userProfile.firstname,
    lastname: $userProfile.lastname,
    email: $userProfile.email
};

let editmode = false, submitting = false, prevForm = JSON.stringify(clientInfo),
changed = false;

$: if(prevForm !== JSON.stringify(clientInfo)) changed = true; else changed = false;


async function SignOutCancel(){
    if(editmode){
        editmode = false;
        clientInfo = JSON.parse(prevForm);
        return;
    }
    localStorage.removeItem("jwt");
    localStorage.removeItem("refresh");
    jwtToken.set('');
    refreshToken.set('');
    userProfile.set(null);
    initShoppingCart(null);
    push('/');
}
function getInputValues() {
    return JSON.stringify(clientInfo);
}
async function onSubmit() {
    await agent.Account.updateProfile(clientInfo);
    userProfile.set(await agent.Account.getProfile());
}
async function edit(){
    if(!editmode){
        editmode = true;
        prevForm = getInputValues();
        return;
    }
    submitting = true;
    await onSubmit();
    submitting = editmode = false;
}

</script>

<div class="container">
    <h1 class="username">{$userProfile.username}</h1>
    <div class="details">
        <div class="firstname">
            {#if editmode}
            <input bind:value={clientInfo.firstname} placeholder="Jan"/>
            {:else}
            <p>{$userProfile.firstname}</p>
            {/if}
        </div>
        <div class="lastname">
            {#if editmode}
            <input bind:value={clientInfo.lastname} placeholder="Kowalski"/>
            {:else}
            <p>{$userProfile.lastname}</p>
            {/if}
        </div>
        <div class="email">
            {#if editmode}
            <input bind:value={clientInfo.email} placeholder="jan@kowalski.com"/>
            {:else}
            <p>{$userProfile.email}</p>
            {/if}
        </div>
    </div>
    <div class="buttons">
        <button class={`${editmode ? 'cancel':'signout'}`} type="button" on:click={SignOutCancel}>
            {#if editmode}
            Cancel
            {:else}
            Sign out
            {/if}
        </button>
        <button class={`edit ${editmode ? !changed ? 'disabled' : 'submit':''}`} type="submit"
        on:click={edit} disabled={editmode && !changed}>
            {editmode? 'Submit' : 'Edit'}
            {#if editmode && submitting}
            <Loader inElement size={1} color={'#ffffff'}/>
            {/if}
        </button>
    </div>
</div>

<style>
    .container {
        background-color: rgb(201, 216, 228);
    }
    .username {
        background-color: aliceblue;
        align-self: flex-start;
        width: 100%;
        height: 20%;
        display: flex;
        justify-content: center;
        align-items: center;
        font-size: 7rem;
        font-family: 'Raleway';
    }
    .details {
        height: 70%;
        width: 40%;
        display: flex;
        justify-content: center;
        flex-direction: column;
        align-items:flex-end;
        padding: 1rem;
        color: rgb(187, 187, 187);
        font-size: 4rem;
        font-family: 'Raleway';
    }
    .details > div {
        position: relative;
        height: auto;
        margin: 1rem;
    }
    .details div *{
        font-family: 'Raleway';
        display: flex;
        justify-content: flex-start;
        align-items: center;
        width: 25rem;
        height: 3rem;
        font-size: 3.5rem;
    }
    .details div input {
        font-size: 2rem !important;
        border: 0.2rem black solid;
        border-radius: 0.5rem;
        text-align: center;
        background-color: transparent;
    }
    .details > div::before {
        position: absolute;
        right: 130%;
        top: -35%;
        color: rgba(114, 114, 114, 0.46);
    }  
    .firstname::before {
        content: 'Firstname';
    }
    .email::before {
        content: 'Email';
    }
    .lastname::before {
        content: 'Lastname';
    }
    .container {
        display: flex;
        flex-direction: column;
        align-items: center;
    }
    .buttons {
        width: 80%;
        height: 8%;
        display: flex;
        align-items: center;
        justify-content: space-evenly;
    }
    .buttons button{
        font-family: 'Raleway';
        width: 30%;
        height: 100%;
        border: none;
        font-size: 3rem;
        border-radius: 1rem;
        display: flex;
        justify-content: center;
        align-items: center;
        transition: all 0.5s ease;
        cursor: pointer;
    }
    .signout {
        background-color: white;
        border: 0.2rem black solid !important;
    }
    .cancel {
        background-color: red;
        color: white;
    }
    .edit {
        background-color: black !important;
        color: white;
        position: relative;
    }
    .submit {
        background-color: rgb(47, 177, 27) !important;
        position: relative;
        color: white;
    }
    .disabled {
        background-color: rgb(151, 151, 151) !important;
        cursor:default !important;
    }
    .submit:hover{
        opacity: 70%;
    }
</style>