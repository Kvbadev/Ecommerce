<script lang="ts">
  import { push } from 'svelte-spa-router';
  import { initShoppingCart } from '../../../Stores/ShoppingCartExtensions';
import {jwtToken, userProfile} from '../../../Stores/stores'

async function SignOut(){
    localStorage.removeItem("jwt");
    jwtToken.set('');
    userProfile.set(null);
    initShoppingCart(null);
    push('/');
}
function edit(){

}

</script>

<div class="container">
    <h1 class="username">{$userProfile.username}</h1>
    <div class="details">
        <div class="firstname">
            <p>{$userProfile.firstname}</p>
        </div>
        <div class="lastname">
            <p>{$userProfile.lastname}</p>
        </div>
        <div class="email">
            <p>{$userProfile.email}</p>
        </div>
    </div>
    <div class="buttons">
        <button class="signout" type="button" on:click={SignOut}>Sign out</button>
        <button class="edit" type="button" on:click={edit}>Edit</button>
    </div>
</div>

<style>
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
    .details div p{
        width: 25rem;
    }
    .firstname::before {
        content: 'Firstname';
        position: absolute;
        left: 6%;
        color: rgba(114, 114, 114, 0.46);
    }
    .email::before {
        content: 'Email';
        position: absolute;
        left: 6%;
        color: rgba(114, 114, 114, 0.46);
    }
    .lastname::before {
        content: 'Lastname';
        position: absolute;
        left: 6%;
        color: rgba(114, 114, 114, 0.46);
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
        /* padding: 2%; */
        border: none;
        font-size: 3rem;
        border-radius: 1rem;
        transition: all 0.5s ease;
        cursor: pointer;
    }
    .signout {
        background-color: white;
        border: 0.2rem black solid !important;
    }
    .edit {
        background-color: black !important;
        color: white;
    }
    .buttons button:hover{
        opacity: 50%;
    }
</style>