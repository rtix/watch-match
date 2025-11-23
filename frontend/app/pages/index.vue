<template>
  <div>
    <h1>Match your watch with your friend now!</h1>
    <p>Can't decide what to watch together? Create room and invite your friend to quickly fix your problem.</p>
    <div class="room-handler">
      <nuxt-link to="/room-creator">
        <ui-button>Create</ui-button>
      </nuxt-link>
      <ui-modal>
        <template #trigger>
          <ui-button>Join</ui-button>
        </template>
        <ui-pin-input @complete="tryJoiningRoom" />
      </ui-modal>
    </div>
  </div>
</template>

<script setup lang="ts">
const router = useRouter();

function tryJoiningRoom(id: string) {
  console.log(`Let me in ${id}!`);
  // Check if exists
  router.push({ name: "room-id", params: { id } })
}
</script>

<style scoped>
.room-handler {
  display: flex;
  gap: 1rem;
  margin-inline-start: 1rem;
}
</style>