from rest_framework import serializers

from Tencu_Framework_Server.models.asset_bundles.Dependency import Dependency


class DependencySerializer(serializers.ModelSerializer):
    class Meta:
        model = Dependency
        fields = ('parent', 'child')
