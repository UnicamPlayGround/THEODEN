from rest_framework import serializers

from Tencu_Framework_Server.models.asset_bundles.AssetBundles import AssetBundles


class AssetBundlesSerializer(serializers.ModelSerializer):
    class Meta:
        model = AssetBundles
        fields = ('id', 'name', 'destination_os', 'file', 'crc')
