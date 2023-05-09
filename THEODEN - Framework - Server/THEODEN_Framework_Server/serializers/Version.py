from rest_framework import serializers

from THEODEN_Framework_Server.models.Version import Version


class VersionSerializer(serializers.ModelSerializer):
    class Meta:
        model = Version
        fields = ('name', 'version')
