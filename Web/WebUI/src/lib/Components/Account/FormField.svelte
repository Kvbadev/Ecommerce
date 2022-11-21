<script lang="ts">

    export let minLen:number, maxLen:number, regex:RegExp|null=null, name = null, isOk:boolean|null = null;
    let inpValue = '', errors = [], isDirty = false;
    export let errorReg = `${name} is not valid`;
    export let errorLen = `${name} length should be smaller than ${maxLen} and bigger than ${minLen}`;
    
    $: inpValue?.length > 0 ? isDirty=true : isDirty=false;

    $: {
        if(minLen && maxLen){
            if(inpValue.length < minLen || inpValue.length > maxLen){
                errors = errors.indexOf(errorLen) == -1 ? [...errors, errorLen] : errors;
            } else {
                errors = errors.filter(v => v !== errorLen);
            }
        }
        if(regex){
            if(!regex.test(inpValue)){
                errors = errors.indexOf(errorReg) == -1 ? [...errors, errorReg] : errors;
            } else {
                errors = errors.filter(v => v !== errorReg);
            }
        }
        errors.length ? isOk=false:isOk=true;
    }
</script>

<span class="form-input">
    {#if name === 'password'}
    <input
        type="password" name={name} placeholder={name[0].toUpperCase()+name.slice(1)}
        bind:value={inpValue} 
        class:error="{errors.length&&isDirty}"
    />
    {:else}
    <input
        type="text" name={name} placeholder={name[0].toUpperCase()+name.slice(1)}
        bind:value={inpValue} 
        class:error="{errors.length&&isDirty}"
    />
    {/if}
    {#if errors.length && isDirty}
        <span class="form-error">
            {#each errors as error}
                <p>{error}</p>
            {/each}
        </span>
    {/if}

</span>

<style>
    .form-input {
        display: flex;
        justify-content: center;
        flex-direction: column;
        align-items: center;
    }
    input {
        margin: 1.2rem;
        font-size: 3vh;
        text-align: center;
        width: 35rem;
        height: 6vh;
        border-radius: 2rem;
        border: 2px solid black;
    }
    .form-error{
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
    .error {
        background-color: rgba(247, 138, 138, 0.2);
    }
</style>