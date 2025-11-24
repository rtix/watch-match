<template>
  <div class="room-handler">
    <nuxt-link to="/room-creator">
      <ui-button>Create</ui-button>
    </nuxt-link>
    <ui-modal>
      <template #trigger>
        <ui-button>Join</ui-button>
      </template>
      <ui-pin-input
        ref="room-handler__pin-input"
        @complete="tryJoiningRoom"
        @value-change="resetErrorMessage"
      />
      <div v-show="errorMessage !== ''" class="room-handler__error-message">
        {{ errorMessage }}
      </div>
    </ui-modal>
  </div>
</template>

<script lang="ts" setup>
const router = useRouter();
const { $roomService } = useNuxtApp();
const roomHandlerPinInput = useTemplateRef("room-handler__pin-input");
const errorMessage = ref("");

function resetErrorMessage() {
  errorMessage.value = "";
}

async function tryJoiningRoom(id: string) {
  const room = await $roomService.getRoom(id);
  if (room === null) {
    roomHandlerPinInput.value?.clear();
    errorMessage.value = "Room not found";
    return;
  }
  router.push({ name: "room-id", params: { id } });
}
</script>

<style scoped>
.room-handler {
  display: flex;
  gap: 1rem;
  margin-inline-start: 1rem;
}
</style>
