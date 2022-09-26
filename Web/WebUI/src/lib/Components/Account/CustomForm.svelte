<script lang="ts">

    import type User from "../../Models/user";
    import { agent } from "../../Utils/agent";
    import { link, push } from "svelte-spa-router";
    import FormField from "./FormField.svelte";
    import { jwtToken, userProfile, shoppingCart} from "../../Stores/stores";
    import Loader from "../Common/Loader.svelte";
    import { initShoppingCart} from "../../Stores/ShoppingCartExtensions";
    import Modal from "./CartModal.svelte";
import { get } from "svelte/store";
    
    export let fields = ['firstname', 'lastname', 'username','email', 'password'];
    export let type: 'Login' | 'Signup';
    
    let isOk:Array<boolean> = [];
    let canSubmit = false;
    let loading = false;
    let showModal = false;

    const onInput = () => {
        if(serverError) serverError='';
        canSubmit = true;
        isOk.forEach(val => {
            if(!val) canSubmit=false;
        })
    }
    
    let serverError:string;
    
    function getFormData(form): User {
        const formData = new FormData(form);
        const data={};
        for(let field of formData){
            const [key, value] = field;
            data[key] = value;
        }
        return data as User;
    }
    
    async function onSubmit (e) {
        loading = true;

        const userData = getFormData(e.target);
        try{
            //get status code and error message / jwt token
            const [status, message] = type === "Signup" ? await agent.Account.signUp(userData) : await agent.Account.logIn(userData);

            if(status !== 200){
                serverError = message.length > 100 ? `Server error: ${status}` : message;
                loading = canSubmit = false;
            } else {
                serverError = '';

                //set jwt in localstorage and in stores
                jwtToken.set(message);
                localStorage.setItem("jwt", message);
                userProfile.set(await agent.Account.getProfile());
                
                if(get(shoppingCart).items.length && type === 'Login'){
                    loading = false;
                    showModal = true;
                } else {
                    initShoppingCart(await agent.ShoppingCart.GetCart());
                    loading = canSubmit = false;
                    push('/');
                }
            }
            
        } catch(err) {
            loading = canSubmit = false;
            console.log(err ?? '');
        }
    }
    
    </script>
    {#if showModal}
    <Modal message="Do you want to persist your shopping cart?" 
           buttons={[{text: 'Persist my cart', type: "good"}, {text: 'Abandon my cart', type: 'bad'}]}
           bind:showModal={showModal}
    />
    {/if}
    <div class="all">
        <div class="container">
        <form class="form" on:submit|preventDefault={onSubmit}>
            <h1>{type==='Signup'?'Sign Up!':'Log In!'}</h1>
            <div class="fields" on:keyup={onInput}>
            <!-- TODO: change input sizes not to be reactive -->
            {#each fields as field, i}
                {#if field === 'email'}
                    <FormField name={field} regex={new RegExp(/^\S+@\S+\.\S+$/)} bind:isOk={isOk[i]} minLen={4} maxLen={50}/>
                {:else}
                    <FormField name={field} minLen={5} maxLen={12} bind:isOk={isOk[i]} />
                {/if}
            {/each}
            </div>
            {#if serverError}
                <span class="form-error">
                    <p>{serverError}</p>
                </span>
            {/if}
            <button class="submit" disabled={!canSubmit || loading} class:disabled="{!canSubmit || loading}" type="submit">
                {#if loading}
                    <Loader size={1} inElement={true} color='white'/>
                {:else} 
                    Submit
                {/if}
            </button>
        </form>
        <div class="links">
            <a href={type === 'Signup'?"/Account/login":"/Account/signup"} use:link>{type === 'Signup' ? "You already have an account?" : "You don't have an account?"}</a>
            <a href="/Account/ForgotPassword" use:link>Forgot your password?</a>
        </div>
    </div>
    </div>
    
    <style>
    
        .links {
            margin-top: 2rem;
            display: flex;
            flex-direction: column;
        }
        h1 {
            font-size: 5rem;
        }
        form {
            border-radius: 1rem;
            width: 100%;
            height: auto;
            display: flex;
            flex-direction: column;
            align-items: center;
        }
        form * {
            margin: 2rem;
        }
        .fields {
            display: flex;
            flex-direction: column;
            justify-content: center;
            align-items: center;
        }
    
        .submit {
            font-family: 'Roboto Slab';
            border: none;
            position: relative;
            display: flex;
            justify-content: center;
            align-items: center;
            border-radius: 2rem;
            width: 25rem;
            height: 5rem;
            cursor: pointer;
            font-size: 2.8rem;
            background-color: yellowgreen;
        }
        a {
            margin-top: auto;
            text-decoration: underline;
            color: blue;
        }
        .container {
            /* margin-bottom: 5rem; */
            font-family: 'Rubik';
            width: auto;
            height: auto;
            display: flex;
            flex-direction: column;
            align-items: center;
            width: 100%;
            height: calc(100vh - 4.2vw);
            min-height: 58rem;
            display: flex;
            justify-content: center;
            align-items: center;
            background-color: white;
        }
        .disabled {
            opacity: 45%;
            cursor:auto;
        }
        .form-error{
            margin: 0;
            text-align: center;
        }
        .form-error p {
            font-family: 'Raleway';
            display: block;
            background-color: rgba(247, 138, 138, 0.2);
            padding: 0.5rem;
            color: red;
            margin: 0.5rem 1rem;
        }
    
    </style>