import { sendRedirect, defineEventHandler } from "h3";
import useLti from "~/composables/use-lti";

export default defineEventHandler(async (event) => {
    const runtimeConfig = useRuntimeConfig();
    const { startLaunch } = useLti();
    const authorizeUrl = await startLaunch(event, runtimeConfig.redirectUri);

    // Fix Safari not redirecting with request parameters
    if (event.headers.get("user-agent")?.includes("Safari")) {
        return `<script>window.location.replace("${authorizeUrl}")</script>`;
    }

    await sendRedirect(event, authorizeUrl);
});
 