<script lang="ts">

    const imgsrc = "src/assets/svelte.svg"

</script>

<li>
    <img src={imgsrc} alt="website logo" />
</li>

<style>
    img {
        height: 7vh;
        width: 7vh;
        /* margin: 10px; */
    }
    li {
        position: absolute;
        left: 50%;
        transform: translateX(-50%);
        cursor: pointer;
    }
</style>